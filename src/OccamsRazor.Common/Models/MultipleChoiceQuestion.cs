using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class MultipleChoiceQuestion : AbstractQuestion
    {
        [JsonPropertyName("answerId")]
        public string AnswerId { get; set; }

        [JsonPropertyName("possibleAnswers")]
        public Dictionary<string, string> PossibleAnswers { get; set; }

        [JsonIgnore]
        public string possibleAnswers 
        {
            get => JsonSerializer.Serialize(PossibleAnswers);
            set => PossibleAnswers = JsonSerializer.Deserialize<Dictionary<string,string>>(value);
        }
    }
}