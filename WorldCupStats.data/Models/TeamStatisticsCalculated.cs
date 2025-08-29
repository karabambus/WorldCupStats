using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Models
{
    public class TeamStatisticsCalculated
    {
        public string Country { get; set; }
        public string FifaCode { get; set; }
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifferential => GoalsFor - GoalsAgainst;
        public int Points => (Wins * 3) + Draws; // Standard FIFA points calculation

        // Static factory method for creating object from match list
        public static TeamStatisticsCalculated CalculateFromMatches(string fifaCode, List<Match> matches)
        {
            if (matches == null || !matches.Any())
            {
                return new TeamStatisticsCalculated
                {
                    FifaCode = fifaCode,
                    Country = "Unknown",
                    GamesPlayed = 0
                };
            }

            var stats = new TeamStatisticsCalculated
            {
                FifaCode = fifaCode,
                GamesPlayed = matches.Count
            };

            int goalsFor = 0;
            int goalsAgainst = 0;
            int wins = 0;
            int losses = 0;
            int draws = 0;

            foreach (var match in matches)
            {
                bool isHomeTeam = match.HomeTeam.Code == fifaCode;

                // Set country name from first match
                if (string.IsNullOrEmpty(stats.Country))
                {
                    stats.Country = isHomeTeam ? match.HomeTeam.Country : match.AwayTeam.Country;
                }

                // Calculate goals
                if (isHomeTeam)
                {
                    goalsFor += (int)match.HomeTeam.Goals;
                    goalsAgainst += (int)match.AwayTeam.Goals;

                    // Determine result
                    if (match.HomeTeam.Goals > match.AwayTeam.Goals)
                        wins++;
                    else if (match.HomeTeam.Goals < match.AwayTeam.Goals)
                        losses++;
                    else
                        draws++;
                }
                else
                {
                    goalsFor += (int)match.AwayTeam.Goals;
                    goalsAgainst += (int)match.HomeTeam.Goals;

                    // Determine result
                    if (match.AwayTeam.Goals > match.HomeTeam.Goals)
                        wins++;
                    else if (match.AwayTeam.Goals < match.HomeTeam.Goals)
                        losses++;
                    else
                        draws++;
                }
            }

            stats.Wins = wins;
            stats.Losses = losses;
            stats.Draws = draws;
            stats.GoalsFor = goalsFor;
            stats.GoalsAgainst = goalsAgainst;

            return stats;
        }

        // Alternativna metoda - ako već imaš Teams objekt za ime
        public static TeamStatisticsCalculated CalculateFromMatches(Teams team, List<Match> matches)
        {
            var stats = CalculateFromMatches(team.FifaCode, matches);
            stats.Country = team.Country; // Koristi ime iz Teams objekta
            return stats;
        }

        
    }
}

