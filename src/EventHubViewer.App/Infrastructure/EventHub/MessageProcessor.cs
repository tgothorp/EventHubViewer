using System;
using Azure.Messaging.EventHubs.Processor;
using EventHubViewer.App.Infrastructure.EventHub.Processors;

namespace EventHubViewer.App.Infrastructure.EventHub
{
    public class MessageProcessor
    {
        public Message ProcessMessage(ProcessEventArgs @event)
        {
            return GetProcessor(@event).Process(@event);
        }

        private IMessageProcessor GetProcessor(ProcessEventArgs @eventArgs)
        {
            return new JsonMessageProcessor();
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
        Raw,
        Json
    }
}