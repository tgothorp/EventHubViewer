using System.Threading.Tasks;
using EventHubViewer.App.Features.Configuration;
using EventHubViewer.App.Infrastructure.Database;
using EventHubViewer.App.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHubViewer.App.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IMediator _mediator;

        public ConfigurationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IActionResult> Index(bool configurationUpdated)
        {
            var model = new ConfigurationModel(await _mediator.Send(new GetConfiguration()));
            
            if (configurationUpdated)
                model.RaiseMessage(Message.MessageLevel.Success, "Configuration updated successfully");

            return View(model);
        }
        
        
        public async Task<IActionResult> UpdateSettings(Configuration configuration)
        {
            await _mediator.Send(new UpdateConfiguration{ Configuration = configuration});
            var updatedConfiguration = await _mediator.Send(new GetConfiguration());
            
            var model = new ConfigurationModel(updatedConfiguration);

            return RedirectToAction("Index", model);
        }
    }
}