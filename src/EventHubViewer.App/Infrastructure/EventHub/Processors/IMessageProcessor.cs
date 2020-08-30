using Azure.Messaging.EventHubs.Processor;

namespace EventHubViewer.App.Infrastructure.EventHub.Processors
{
    public interface IMessageProcessor
    {
        Message Process(ProcessEventArgs @event);
    }
}