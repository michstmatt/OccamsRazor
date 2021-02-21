using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace OccamsRazor.Common.Models
{
    public class Question : AbstractQuestion
    {
        [JsonPropertyName("answerText")]
        public string AnswerText { get; set; }
    }
}