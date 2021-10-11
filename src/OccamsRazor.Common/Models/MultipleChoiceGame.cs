using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class MultipleChoiceGame : AbstractGame
    {
        [JsonPropertyName("questions")]
        public List<MultipleChoiceQuestion> Questions { get; set; }

        public MultipleChoiceGame()
        {
            Metadata = new GameMetadata() { State = GameStateEnum.Created };
            Format = new Dictionary<RoundEnum, int>
            {
                {RoundEnum.One, 3},
                {RoundEnum.Two, 3},
                {RoundEnum.Three, 3},
                {RoundEnum.HalfTime, 1},
                {RoundEnum.Four, 3},
                {RoundEnum.Five, 3},
                {RoundEnum.Six, 3},
                {RoundEnum.Final, 1},
            };
            Questions = new List<MultipleChoiceQuestion>();
        }

    }
}