using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;

namespace WorldCupStats.data.Services
{
    public class PlayerService
    {
        private readonly IDataRepository _repository;

        public PlayerService(IDataRepository repository)
        {
            _repository = new FileDataRepository(SettingsManager.IsApiMode());

        }

        public async Task<TeamLoadResult> LoadTeamsAsync()
        {
            try
            {
                var teams = await _repository.GetTeamsAsync(SettingsManager.GetChampionship());
                var sortedTeams = teams.OrderBy(t => t.Country).ToList();
                var favoriteTeamCode = SettingsManager.GetFavoriteTeam();

                return new TeamLoadResult
                {
                    Success = true,
                    Teams = sortedTeams,
                    FavoriteTeamCode = favoriteTeamCode
                };
            }
            catch (Exception ex)
            {
                return new TeamLoadResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<PlayerLoadResult> LoadPlayersAsync(string fifaCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fifaCode))
                    throw new ArgumentException("FIFA code cannot be empty");

                // Get all matches for the team
                var matches = await _repository.GetMatchesByTeamAsync(SettingsManager.GetChampionship(), fifaCode);
                if (matches == null || !matches.Any())
                {
                    return new PlayerLoadResult
                    {
                        Success = false,
                        ErrorMessage = "Nema utakmica za odabrani tim!",
                        TeamCode = fifaCode
                    };
                }

                // Get players from first match
                var playersResult = ExtractPlayersFromMatch(matches.First(), fifaCode);
                if (!playersResult.Success)
                {
                    return playersResult;
                }

                // Save preferred team
                SettingsManager.SetFavoriteTeam(fifaCode);

                // Load favorite players list (don't modify player objects)
                var favoritePlayers = SettingsManager.GetFavoritePlayers(fifaCode) ?? new List<int>();

                return new PlayerLoadResult
                {
                    Success = true,
                    Players = playersResult.Players,
                    FavoritePlayers = favoritePlayers, // Add this to result
                    TeamCode = fifaCode,
                    StartingElevenCount = playersResult.StartingElevenCount,
                    SubstitutesCount = playersResult.SubstitutesCount
                };
            }
            catch (Exception ex)
            {
                return new PlayerLoadResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    TeamCode = fifaCode
                };
            }
        }

        private PlayerLoadResult ExtractPlayersFromMatch(Match match, string fifaCode)
        {
            // Check if home or away team
            bool isHome = match.HomeTeam?.Code == fifaCode;
            TeamStatistics teamStats = isHome ? match.HomeTeamStatistics : match.AwayTeamStatistics;

            if (teamStats == null)
            {
                return new PlayerLoadResult
                {
                    Success = false,
                    ErrorMessage = $"Nema statistike za tim {fifaCode} u prvoj utakmici!",
                    TeamCode = fifaCode
                };
            }

            List<StartingEleven> allPlayers = new List<StartingEleven>();
            int startingCount = 0, substitutesCount = 0;

            // Add starting eleven
            if (teamStats.StartingEleven != null && teamStats.StartingEleven.Any())
            {
                allPlayers.AddRange(teamStats.StartingEleven);
                startingCount = teamStats.StartingEleven.Count;
            }

            // Add substitutes
            if (teamStats.Substitutes != null && teamStats.Substitutes.Any())
            {
                allPlayers.AddRange(teamStats.Substitutes);
                substitutesCount = teamStats.Substitutes.Count;
            }

            if (!allPlayers.Any())
            {
                return new PlayerLoadResult
                {
                    Success = false,
                    ErrorMessage = $"Nema podataka o igračima za tim {fifaCode}!\n" +
                                  $"Utakmica: {match.HomeTeamCountry} vs {match.AwayTeamCountry}\n" +
                                  $"Tim je {(isHome ? "domaćin" : "gost")}",
                    TeamCode = fifaCode
                };
            }

            return new PlayerLoadResult
            {
                Success = true,
                Players = allPlayers,
                TeamCode = fifaCode,
                StartingElevenCount = startingCount,
                SubstitutesCount = substitutesCount
            };
        }

        public void TogglePlayerFavorite(string teamCode, int playerId, bool isFavorite)
        {
            var favorites = SettingsManager.GetFavoritePlayers(teamCode)?.ToList() ?? new List<int>();

            if (isFavorite && !favorites.Contains(playerId))
            {
                favorites.Add(playerId);
            }
            else if (!isFavorite)
            {
                favorites.Remove(playerId);
            }

            SettingsManager.SetFavoritePlayers(teamCode, favorites);
        }
    }

    //result classes
    public class TeamLoadResult
    {
        public bool Success { get; set; }
        public List<Teams> Teams { get; set; } = new();
        public string FavoriteTeamCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PlayerLoadResult
    {
        public bool Success { get; set; }
        public List<StartingEleven> Players { get; set; } = new();
        public List<int> FavoritePlayers { get; set; } = new(); 
        public string TeamCode { get; set; }
        public string ErrorMessage { get; set; }
        public int StartingElevenCount { get; set; }
        public int SubstitutesCount { get; set; }
    }
}
