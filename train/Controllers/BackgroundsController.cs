using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using train.Model;
using train.Servise;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace train.Controllers
{
	public class BackgroundsController : Controller
	{
		private readonly IBackgrounds _backgroundService;
		public BackgroundsController(IBackgrounds backgrounds) {
			_backgroundService = backgrounds;
		}

        public async Task<IActionResult> Index()
        {


            return View();
        }

        // GET: api/<BackgroundsController>
        [HttpPost]
		public IActionResult Post(Background B)
		{
			Console.WriteLine("sss");
			/*_backgroundService.SetBack(B);*/
			return Ok();
		}

	}
}
