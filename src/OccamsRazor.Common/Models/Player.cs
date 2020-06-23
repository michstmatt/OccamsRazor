using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class Player
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}