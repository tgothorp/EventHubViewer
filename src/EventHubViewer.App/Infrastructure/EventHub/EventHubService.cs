using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using EventHubViewer.App.Features.Configuration;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace EventHubViewer.App.Infrastructure.EventHub
{
    public class EventHubService : IHostedService, IDisposable
    {
        public bool IsRunning { get; set; }
        
        private readonly IMediator _mediator;
        private readonly IHubContext<EventHubSignalHub> _hubContext;
        private readonly MessageProcessor _messageProcessor;

        public EventHubService() { }
        
        public EventHubService(IMediator mediator, IHubContext<EventHubSignalHub> hubContext, MessageProcessor messageProcessor)
        {
            _mediator = mediator;
            _hubContext = hubContext;
            _messageProcessor = messageProcessor;
        }
        
        /// <summary>
        /// This should be automatically started by the runtime
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;

            var configuration = await _mediator.Send(new GetConfiguration(), cancellationToken);
            if (!configuration.CheckConfigurationIsValid())
                return;

            var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            var storageClient = new BlobContainerClient(configuration.BlobStorageConnectionString, configuration.BlobContainerName);
            var processor = new EventProcessorClient(storageClient, consumerGroup, configuration.EventHubConnectionString, configuration.EvenHubName);

            processor.ProcessEventAsync += EventReceived;
            processor.ProcessErrorAsync += ErrorReceived;
            
            await processor.StartProcessingAsync(cancellationToken);
        }

        private Task ErrorReceived(ProcessErrorEventArgs arg) { return Task.CompletedTask; }

        private async Task EventReceived(ProcessEventArgs arg)
        {
            await _hubContext.Clients.All.SendAsync("MessageReceived", _messageProcessor.ProcessMessage(arg));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}