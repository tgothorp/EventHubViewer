using System.Threading;
using System.Threading.Tasks;
using EventHubViewer.App.Infrastructure.Database;
using EventHubViewer.App.Infrastructure.EventHub;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace EventHubViewer.App.Features.Configuration
{
    public class UpdateConfiguration : IRequest<Unit>
    {
        public Infrastructure.Database.Configuration Configuration { get; set; }
        
        public class Handler : IRequestHandler<UpdateConfiguration, Unit>
        {
            private readonly ConfigurationDatabase _configurationDatabase;
            private readonly EventHubService _eventHubService;

            public Handler(ConfigurationDatabase configurationDatabase, IHostedService eventHubService)
            {
                _configurationDatabase = configurationDatabase;
                _eventHubService = eventHubService as EventHubService;
            }
            
            public async Task<Unit> Handle(UpdateConfiguration request, CancellationToken cancellationToken)
            {
                _configurationDatabase.UpdateConfiguration(request.Configuration);

                await _eventHubService.StopAsync(cancellationToken);
                await _eventHubService.StartAsync(new CancellationToken());

                return Unit.Value;
            }
        }
    }
}