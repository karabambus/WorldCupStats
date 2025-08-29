using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Models
{
    public class TeamWithStats
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("alternate_name")]
        public string? AlternateName { get; set; }

        [JsonProperty("fifa_code")]
        public string FifaCode { get; set; }

        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        [JsonProperty("group_letter")]
        public string GroupLetter { get; set; }

        // Performance Statistics
        [JsonProperty("wins")]
        public int Wins { get; set; }

        [JsonProperty("draws")]
        public int Draws { get; set; }

        [JsonProperty("losses")]
        public int Losses { get; set; }

        [JsonProperty("games_played")]
        public int GamesPlayed { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("goals_for")]
        public int GoalsFor { get; set; }

        [JsonProperty("goals_against")]
        public int GoalsAgainst { get; set; }

        [JsonProperty("goal_differential")]
        public int GoalDifferential { get; set; }

        // Calculated Properties (not in JSON)
        [JsonIgnore]
        public double WinPercentage => GamesPlayed > 0 ? (double)Wins / GamesPlayed * 100 : 0;

        [JsonIgnore]
        public double PointsPerGame => GamesPlayed > 0 ? (double)Points / GamesPlayed : 0;

        //obavezno 
        [JsonIgnore]
        public double GoalsPerGame => GamesPlayed > 0 ? (double)GoalsFor / GamesPlayed : 0;

        [JsonIgnore]
        public double GoalsAgainstPerGame => GamesPlayed > 0 ? (double)GoalsAgainst / GamesPlayed : 0;

        [JsonIgnore]
        public bool IsDataConsistent => Wins + Draws + Losses == GamesPlayed;
    }
}
