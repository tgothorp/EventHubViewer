using System;
using System.Text;
using Azure.Messaging.EventHubs.Processor;

namespace EventHubViewer.App.Infrastructure.EventHub.Processors
{
    public class RawMessageProcessor : IMessageProcessor
    {
        public Message Process(ProcessEventArgs @event)
        {
            return new RawMessage(@event);
        }
        
        public class RawMessage : Message
        {
            public RawMessage(ProcessEventArgs @event)
            {
                Enqueued = @event.Data.EnqueuedTime.DateTime.ToString("G");
                Received = DateTime.Now.ToString("G");
                RawMessage = Encoding.UTF8.GetString(@event.Data.Body.ToArray());
                MessageFormat = MessageFormat.Unknown;
                MessageLength = @event.Data.Body.Length;
            }
        }
    }
}