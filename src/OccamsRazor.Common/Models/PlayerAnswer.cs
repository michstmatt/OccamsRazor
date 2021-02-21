using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class PlayerAnswer : AbstractPlayerAnswer
    {
        [JsonPropertyName("answerText")]
        public string AnswerText { get; set; }
        [JsonPropertyName("wager")]
        public int Wager { get; set; }
    }
}