using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class GameMetadata
    {
        [JsonPropertyName("gameId")]
        public int GameId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("currentRound")]
        public RoundEnum CurrentRound { get; set; } = RoundEnum.One;

        [JsonPropertyName("currentQuestion")]
        public int CurrentQuestion { get; set; } = 1;

        [JsonPropertyName("state")]
        public GameStateEnum State {get;set;}

    }
}