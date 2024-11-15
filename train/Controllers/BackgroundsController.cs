using Microsoft.AspNetCore.Mvc;
using train.Model;
using train.Servise;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace train.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BackgroundsController : ControllerBase
	{
		private readonly IBackgrounds _backgroundService;
		public BackgroundsController(IBackgrounds backgrounds) {
			_backgroundService = backgrounds;
		}
		// GET: api/<BackgroundsController>
		[HttpPost]
		public IActionResult Post(Background B)
		{
			_backgroundService.SetBack(B);
			return Ok();
		}

	}
}
