namespace OccamsRazor.Models
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public enum MessageType
    {
        StateMessage = 0,
        AnswerMessage = 1,

    }

    public static class MessageExtensions
    {
        public static Type GetMessageType(this MessageType t)
        {
            if (t == MessageType.StateMessage) return typeof(StateMessage);
            if (t == MessageType.AnswerMessage) return typeof(AnswerMessage);
            throw new Exception("Unexpected Enum type");
        }
    }


    public abstract class AbstractMessage
    {
        public MessageType Type { get; protected set; }
        public AbstractMessage(MessageType type) => this.Type = type;

        public async Task<System.IO.Stream> ToJson()
        {
            using var stream = new System.IO.MemoryStream();
            await JsonSerializer.SerializeAsync(stream, this);
            return stream;
        }
    }

    public class StateMessage : AbstractMessage
    {
        public StateMessage() : base(MessageType.StateMessage) { }
        public GameStateEnum State { get; set;}
    }

    public class AnswerMessage : AbstractMessage
    {
        public AnswerMessage() : base(MessageType.AnswerMessage) { }
        public string Answer { get; set; }
    }
}