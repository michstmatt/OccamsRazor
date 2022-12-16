namespace OccamsRazor.Models
{
    using System.Collections.Generic;
    public class Game
    {
        public readonly string Id;
        public readonly string HostPassword;
        public readonly string JoinKey;

        public GameStateEnum State;

        public uint CurrentQuestion = 0;
        public List<Question> Questions { get; set; }

        public Game(string id, string hostPassword, string joinKey)
        {
            this.Id = id;
            this.HostPassword = hostPassword;
            this.JoinKey = joinKey;
            this.State = GameStateEnum.Created;
        }
    }
}