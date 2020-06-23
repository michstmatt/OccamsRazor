using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OccamsRazor.Common.Models
{
    public class Game
    {
        [JsonPropertyName("id")]
        public string Id { get => this.Metadata.GameId.ToString(); set { } }

        [JsonPropertyName("rounds")]
        public List<GameRound> Rounds { get; set; }

        [JsonPropertyName("metadata")]
        public GameMetadata Metadata { get; set; }

        public Game()
        {
            Metadata = new GameMetadata();
            var format = new Dictionary<RoundEnum, int>
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

            Rounds = new List<GameRound>();

            foreach (var pair in format)
            {
                var round = new GameRound { Round = pair.Key };
                round.Questions = new List<Question>();
                for (int i = 0; i < pair.Value; i++)
                {
                    round.Questions.Add(new Question { Round = pair.Key, Number = i + 1, Text = "" });
                }
                Rounds.Add(round);
            }
        }
    }
}