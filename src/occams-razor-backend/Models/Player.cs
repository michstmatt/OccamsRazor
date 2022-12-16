namespace OccamsRazor.Models
{
    using System;
    using System.Security.Claims;

    public enum PlayerRole
    {
        Player,
        GameAdmin
    }

    public class Player
    {
        public string Name {get; private set;}
        public string GameId {get; private set;}
        public Guid Id {get; private set;}
        public PlayerRole Role {get; private set;}

        public Player(string name, string gameId, PlayerRole role = PlayerRole.Player)
        {
            this.Name = name;
            this.GameId = gameId;
            this.Id = Guid.NewGuid();
            this.Role = role;
        }
        public Player(string name, string gameId, Guid id, PlayerRole role = PlayerRole.Player)
        {
            this.Name = name;
            this.GameId = gameId;
            this.Id = id;
            this.Role = role;
        }
    }    
}