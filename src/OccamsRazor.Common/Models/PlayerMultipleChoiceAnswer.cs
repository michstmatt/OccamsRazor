using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class PlayerMultipleChoiceAnswer : AbstractPlayerAnswer
    {
        [JsonPropertyName("answerId")]
        public string AnswerId{ get; set; }
    }
}