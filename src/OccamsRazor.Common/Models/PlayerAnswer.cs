using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class PlayerAnswer
    {
        [JsonIgnore]
        public int Id {get;set;}

        [JsonPropertyName("player")]
        public Player Player { get; set; }

        [JsonPropertyName("answerText")]
        public string AnswerText { get; set; }
        [JsonPropertyName("wager")]
        public int Wager { get; set; }

        [JsonPropertyName("questionNumber")]
        public int QuestionNumber { get; set; }

        [JsonPropertyName("round")]
        public RoundEnum Round { get; set; }

        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [JsonPropertyName("pointsAwarded")]
        public int pointsAwarded { get; set; }
    }
}