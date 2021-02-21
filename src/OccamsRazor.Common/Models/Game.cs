using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class Game
    {
        [JsonPropertyName("questions")]
        public List<AbstractQuestion> Questions { get; set; }

        [JsonPropertyName("metadata")]
        public GameMetadata Metadata { get; set; }
        [JsonIgnore]
        public Dictionary<RoundEnum, int> Format { get; set; }
        public Game()
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
            Questions = new List<AbstractQuestion>();
            /*
                        Questions = new List<Question>();

                        foreach (var pair in format)
                        {
                            var round = pair.Key;
                            for (int i = 0; i < pair.Value; i++)
                            {
                                Questions.Add(new Question { Round = pair.Key, Number = i + 1, Text = "", AnswerText = "", Category = "" });
                            }

                        }*/
        }

    }
}