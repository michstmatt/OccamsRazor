using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class GameResults
    {
        [JsonPropertyName("player")]
        public Player Player { get; set; }

        [JsonPropertyName("playerAnswers")]
        public List<PlayerAnswer> PlayerAnswers {get; set;}

        [JsonPropertyName("totalScore")]
        public int TotalScore {get;set;}
    }
}