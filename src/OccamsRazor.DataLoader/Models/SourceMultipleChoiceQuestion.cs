using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.DataLoader.Models
{
    public class SourceAnswers
    {
        [JsonPropertyName("text")]
        public string Text {get; set;}

        [JsonPropertyName("correct")]
        public bool Correct {get; set;}

        [JsonPropertyName("choice")]
        public string Choice {get; set;}

    }
    public class SourceMultipleChoiceQuestion
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }

        [JsonPropertyName("question_num")]
        public int QuestionNum { get; set; }

        [JsonPropertyName("answers")]
        public List<SourceAnswers> PossibleAnswers { get; set; }
    }
}