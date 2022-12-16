namespace OccamsRazor.Models
{
    using System;
    public class PlayerAnswer
    {
        public string GameId {get; set;}
        public uint QuestionId {get; set;}
        public Guid PlayerId {get; set;}
        public string Answer {get; set;}
    }
}