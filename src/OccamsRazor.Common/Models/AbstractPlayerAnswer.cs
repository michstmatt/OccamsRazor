using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class AbstractPlayerAnswer
    {
        [JsonIgnore]
        public int Id {get;set;}

        [JsonPropertyName("player")]
        public Player Player { get; set; }

        [JsonPropertyName("questionNumber")]
        public int QuestionNumber { get; set; }

        [JsonPropertyName("round")]
        public RoundEnum Round { get; set; }

        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [JsonPropertyName("pointsAwarded")]
        public int PointsAwarded { get; set; }
    }
}