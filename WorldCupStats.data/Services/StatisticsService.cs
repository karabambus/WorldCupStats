using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupStats.data.Exceptions;
using WorldCupStats.data.Helpers;
using WorldCupStats.data.Models;
using WorldCupStats.data.Repository;

namespace WorldCupStats.data.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDataRepository _dataRepository;
        private readonly string _championship;

        public StatisticsService(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
            _championship = SettingsManager.GetChampionship();
        }

        // Alternative constructor to override championship
        public StatisticsService(IDataRepository repository, string championship)
        {
            _dataRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _championship = championship;
        }

        public async Task<List<Teams>> GetAllTeamsAsync()
        {
            try
            {
                return await _dataRepository.GetTeamsAsync(_championship);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to retrieve teams", ex);
            }
        }



        public async Task<List<Match>> GetMatchesByAttendanceAsync(int count = 10)
        {
            try
            {
                var allMatches = await _dataRepository.GetMatchesAsync(_championship);
                return allMatches
                .OrderByDescending(m => m.Attendance)
                .Take(count)
                .ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to get matches by attendance", ex);
            }
        }

        public async Task<List<PlayerStatistics>> GetMostYellowCardsAsync(int count = 10)
        {
            try
            {
                var allMatches = await _dataRepository.GetMatchesAsync(_championship);
                // Use the extension method from PlayerStatistics class
                var allPlayers = PlayerStatistics
                    .GetAllPlayersStatistics(allMatches)
                    .Values
                    .ToList();

                return allPlayers
                    .Where(p => p.YellowCards > 0)
                    .OrderByDescending(p => p.YellowCards)
                    .ThenBy(p => p.Name)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to get yellow cards", ex);
            }
        }

        public async Task<List<PlayerStatistics>> GetPlayerStatisticsByTeamAsync(string fifaCode)
        {
            try
            { 
                //checks
                if (string.IsNullOrEmpty(fifaCode))
                    throw new ArgumentException("FIFA code cannot be null or empty.", nameof(fifaCode));

                // Get all matches for the team
                var matches = await _dataRepository.GetMatchesByTeamAsync(_championship, fifaCode);

                var favoriteNumbers = SettingsManager.GetFavoritePlayers(fifaCode);
                // Use the static method from PlayerStatistics class
                var playerStats = PlayerStatistics.CalculateForTeam(matches, fifaCode);

                return playerStats.OrderBy(p => p.Name).ToList();
            }
            catch (Exception ex)
            {

                throw new ServiceException($"Failed to get player statistics for team {fifaCode}", ex);
            }
        }

        public async Task<List<Match>> GetTeamMatchesAsync(string fifaCode)
        {
            try
            {
                return await _dataRepository.GetMatchesByTeamAsync(_championship, fifaCode);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Failed to get matches for team {fifaCode}", ex);
            }
        }

        public async Task<TeamStatisticsCalculated> GetTeamStatisticsCalculatedAsync(string fifaCode)
        {
            try
            {
                var matches = await GetTeamMatchesAsync(fifaCode);
                
                return TeamStatisticsCalculated.CalculateFromMatches(fifaCode, matches);
            }
            catch (Exception ex)
            {
                throw new ServiceException($"Error getting team statistics: {ex.Message}", ex);
            }
        }

        public async Task<List<PlayerStatistics>> GetTopScorersAsync(int count = 10)
        {
            try
            {
                var allMatches = await _dataRepository.GetMatchesAsync(_championship);
                // Use the extension method from PlayerStatistics class
                var allPlayers = PlayerStatistics
                    .GetAllPlayersStatistics(allMatches)
                    .Values
                    .ToList();

                return allPlayers
                    .Where(p => p.Goals > 0)
                    .OrderByDescending(p => p.Goals)
                    .ThenBy(p => p.Name)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Failed to get top scorers", ex);
            }
        }
    }
}
