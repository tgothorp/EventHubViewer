using Microsoft.AspNetCore.Mvc;

namespace EventHubViewer.App.Models
{
    public class BaseModel
    {
        public BaseModel(string title)
        {
            Title = title;
        }
        
        public string Title { get; set; }
        
        [TempData]
        public Message Message { get; private set; }

        public void RaiseMessage(Message.MessageLevel messageLevel, string body) => Message = new Message(messageLevel, body);
    }

    public class Message
    {
        public Message(MessageLevel level, string body)
        {
            Level = level;
            Body = body;
        }
        
        public string Body { get; set; }
        
        public MessageLevel Level { get; set; }
        
        public enum MessageLevel
        {
            Info,
            Warning,
            Error,
            Success,
        }
    }
}