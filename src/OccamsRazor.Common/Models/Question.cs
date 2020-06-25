using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace OccamsRazor.Common.Models
{
    public class Question
    {
        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("round")]
        public RoundEnum Round { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }
    }
}