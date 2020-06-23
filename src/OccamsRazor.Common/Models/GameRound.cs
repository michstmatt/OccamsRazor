using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace OccamsRazor.Common.Models
{
    public class GameRound
    {
        [JsonPropertyName("Round")]
        public RoundEnum Round { get; set; }
        [JsonPropertyName("questions")]
        public List<Question> Questions { get; set; }
    }
}