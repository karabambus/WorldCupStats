using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Models
{ 
public class Group
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("letter")]
        public string Letter { get; set; }

        [JsonProperty("ordered_teams")]
        public List<TeamWithStats> OrderedTeams { get; set; }

        // Calculated properties for group analysis  
        [JsonIgnore]
        public TeamWithStats Winner => OrderedTeams?.FirstOrDefault();

        [JsonIgnore]
        public TeamWithStats RunnerUp => OrderedTeams?.Skip(1).FirstOrDefault();

        [JsonIgnore]
        public List<TeamWithStats> QualifyingTeams => OrderedTeams?.Take(2).ToList() ?? new List<TeamWithStats>();

        [JsonIgnore]
        public List<TeamWithStats> EliminatedTeams => OrderedTeams?.Skip(2).ToList() ?? new List<TeamWithStats>();

        [JsonIgnore]
        public int TotalGoalsScored => OrderedTeams?.Sum(t => t.GoalsFor) ?? 0;

        [JsonIgnore]
        public int TotalGamesPlayed => OrderedTeams?.Sum(t => t.GamesPlayed) ?? 0;

        [JsonIgnore]
        public double AverageGoalsPerGame => TotalGamesPlayed > 0 ? (double)TotalGoalsScored / TotalGamesPlayed : 0;

        [JsonIgnore]
        public bool IsCompetitive => OrderedTeams?.Count >= 2 &&
                                      Math.Abs(OrderedTeams[0].Points - OrderedTeams[1].Points) <= 3;
    }

}
