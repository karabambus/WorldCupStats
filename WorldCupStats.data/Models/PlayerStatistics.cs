using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Models
{
    public class PlayerStatistics
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public Position Position { get; set; }
        public int Goals { get; set; }
        public int YellowCards { get; set; }
        public int Appearances { get; set; }  // Games played
        public bool Captain { get; set; }
        public string ImagePath { get; set; }
        public bool IsFavorite { get; set; }

        public int ShirtNumber { get; set; }

        public string FifaCode { get; set; } // Added to identify the team

        //Calculate statistics from all matches from specific team
        public static List<PlayerStatistics> CalculateForTeam(List<Match> matches, string fifaCode)
        {
            var playerStatsDict = new Dictionary<string, PlayerStatistics>();

            foreach (var match in matches)
            {
                TeamStatistics teamstats = null;
                List<TeamEvent> teamevents = null;
                 
                // Determine if we are looking at home or away team and save fifaCode for save image
                if (match.HomeTeam?.Code == fifaCode)
                {
                    fifaCode = match.HomeTeam.Code;
                    teamstats = match.HomeTeamStatistics;
                    teamevents = match.HomeTeamEvents;
                }
                else if (match.AwayTeam?.Code == fifaCode)
                {
                    fifaCode = match.AwayTeam.Code;
                    teamstats = match.AwayTeamStatistics;
                    teamevents = match.AwayTeamEvents;
                }

                //process players in the starting eleven
                if (teamstats.StartingEleven != null)
                {
                    foreach (var player in teamstats.StartingEleven)
                    {
                        int shirtNumber = player.ShirtNumber;
                        if (!playerStatsDict.ContainsKey(player.Name))
                        {
                            playerStatsDict[player.Name] = new PlayerStatistics
                            {
                                Name = player.Name,
                                Country = teamstats.Country,
                                Position = player.Position,
                                Captain = player.Captain,
                                ShirtNumber = shirtNumber,
                                FifaCode = fifaCode
                            };
                        }
                        // Increment appearance
                        playerStatsDict[player.Name].Appearances++;
                    }

                    //process players in the substitutes

                    if (teamstats.Substitutes != null)
                    {
                        foreach (var player in teamstats.Substitutes)
                        {
                            int shirtNumber = player.ShirtNumber;
                            if (!playerStatsDict.ContainsKey(player.Name))
                            {
                                playerStatsDict[player.Name] = new PlayerStatistics
                                {
                                    Name = player.Name,
                                    Country = teamstats.Country,
                                    Position = player.Position,
                                    Captain = player.Captain,
                                    ShirtNumber = shirtNumber,
                                    FifaCode = fifaCode
                                };
                            }

                            bool playerParticipated = teamevents?.Any(e =>
                                e.Player == player.Name &&
                                e.TypeOfEvent == TypeOfEvent.SubstitutionIn) ?? false;

                            if (playerParticipated)
                                playerStatsDict[player.Name].Appearances++;
                        }


                    }

                    if (teamevents != null)
                    {
                        foreach (var evt in teamevents)
                        {
                            if (playerStatsDict.ContainsKey(evt.Player))
                            {
                                switch (evt.TypeOfEvent)
                                {
                                    case TypeOfEvent.Goal:
                                    case TypeOfEvent.GoalPenalty:
                                        playerStatsDict[evt.Player].Goals++;
                                        break;
                                    case TypeOfEvent.GoalOwn:  // Don't count own goals
                                        break;
                                    case TypeOfEvent.YellowCard:
                                    case TypeOfEvent.YellowCardSecond:
                                        playerStatsDict[evt.Player].YellowCards++;
                                        break;
                                }
                            }
                        }
                    }
                }

            }
            return playerStatsDict.Values
                .OrderBy(p => p.Name)
                .ToList();
        }


        // Helper method to get top scorers
        public static List<PlayerStatistics> GetTopScorers(List<PlayerStatistics> players, int count = 10)
        {
            return players
                .OrderByDescending(p => p.Goals)
                .ThenBy(p => p.Name)
                .Take(count)
                .ToList();
        }

        // Helper method to get players with most yellow cards
        public static List<PlayerStatistics> GetMostYellowCards(List<PlayerStatistics> players, int count = 10)
        {
            return players
                .OrderByDescending(p => p.YellowCards)
                .ThenBy(p => p.Name)
                .Take(count)
                .ToList();
        }

        public static Dictionary<string, PlayerStatistics> GetAllPlayersStatistics(List<Match> matches)
        {
            var allPlayerStats = new Dictionary<string, PlayerStatistics>();
            var teamCodes = GetUniqueTeamCodes(matches);

            foreach (var teamCode in teamCodes)
            {
                var teamMatches = matches.Where(m =>
                    m.HomeTeam?.Code == teamCode ||
                    m.AwayTeam?.Code == teamCode).ToList();

                var teamPlayers = PlayerStatistics.CalculateForTeam(teamMatches, teamCode);

                foreach (var player in teamPlayers)
                {
                    // Use composite key: TeamCode_PlayerName
                    var key = $"{teamCode}_{player.Name}";
                    allPlayerStats[key] = player;
                }
            }

            return allPlayerStats;
        }
        //this needs to just use Teams.json
        private static HashSet<string> GetUniqueTeamCodes(List<Match> matches)
        {
            var teamCodes = new HashSet<string>();
            foreach (var match in matches)
            {
                if (match.HomeTeam?.Code != null)
                    teamCodes.Add(match.HomeTeam.Code);
                if (match.AwayTeam?.Code != null)
                    teamCodes.Add(match.AwayTeam.Code);
            }
            return teamCodes;
        }
    }

}
