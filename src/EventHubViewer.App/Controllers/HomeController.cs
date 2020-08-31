using System.Threading;
using System.Threading.Tasks;
using EventHubViewer.App.Infrastructure.EventHub;
using EventHubViewer.App.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace EventHubViewer.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly EventHubService _eventHubService;

        public HomeController(IMediator mediator, IHostedService eventHubService)
        {
            _mediator = mediator;
            _eventHubService = eventHubService as EventHubService;
        }
        
        // GET
        public IActionResult Index()
        {
            return View(new BaseModel("Home"));
        }
    }
}