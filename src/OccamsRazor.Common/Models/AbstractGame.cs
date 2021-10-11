using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public abstract class AbstractGame
    {

        [JsonPropertyName("metadata")]
        public GameMetadata Metadata { get; set; }
        [JsonIgnore]
        public Dictionary<RoundEnum, int> Format { get; set; }
    }
}