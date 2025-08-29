using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.data.Models
{
    public class Player 
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("captain")]
        public bool Captain { get; set; }

        [JsonProperty("shirt_number")]
        public int ShirtNumber { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        public override string ToString()
        {
            return $"{Name} (#{ShirtNumber}) - {Position} {(Captain ? "(C)" : "")}"; // Returns a formatted string representation of the player  
        }
    }
}
