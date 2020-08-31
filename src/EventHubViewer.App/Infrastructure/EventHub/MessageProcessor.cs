using System;
using System.Text;
using Azure.Messaging.EventHubs.Processor;
using EventHubViewer.App.Extensions;
using EventHubViewer.App.Infrastructure.EventHub.Processors;

namespace EventHubViewer.App.Infrastructure.EventHub
{
    public class MessageProcessor
    {
        public Message ProcessMessage(ProcessEventArgs @event)
        {
            return GetProcessor(@event).Process(@event);
        }

        private IMessageProcessor GetProcessor(ProcessEventArgs @event)
        {
            var rawMessage = Encoding.UTF8.GetString(@event.Data.Body.ToArray());
            
            if (rawMessage.IsValidJson())
                return new JsonMessageProcessor();
            
            return new RawMessageProcessor();
        }
    }

    public abstract class Message
    {
        public string Enqueued { get; set; }
        public string Received { get; set; }
        public string RawMessage { get; set; }
        public long MessageLength { get; set; }

        public MessageFormat MessageFormat { get; set; }
    }

    public enum MessageFormat
    {
        Unknown,
        Json
    }
}