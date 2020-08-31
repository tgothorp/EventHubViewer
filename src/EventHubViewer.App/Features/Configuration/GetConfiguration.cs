using System.Threading;
using System.Threading.Tasks;
using EventHubViewer.App.Infrastructure.Database;
using MediatR;

namespace EventHubViewer.App.Features.Configuration
{
    public class GetConfiguration : IRequest<Infrastructure.Database.Configuration>
    {
        public class Handler : IRequestHandler<GetConfiguration, Infrastructure.Database.Configuration>
        {
            private readonly ConfigurationDatabase _configurationDatabase;

            public Handler(ConfigurationDatabase configurationDatabase)
            {
                _configurationDatabase = configurationDatabase;
            }
            
            public Task<Infrastructure.Database.Configuration> Handle(GetConfiguration request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_configurationDatabase.GetConfiguration());
            }
        }
    }
}