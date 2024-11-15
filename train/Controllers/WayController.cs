using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using train.Model;
using train.Servise;

namespace train.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WayController : ControllerBase
	{
		private readonly WayGen _WayGen;
		private readonly ILogger<WayController> _logger;
		public WayController(WayGen wayGen, ILogger<WayController> logger)
		{
			_WayGen = wayGen;
			_logger = logger;
		}

		[HttpGet]
		public ActionResult<WayModel> Get()
		{
			_logger.LogInformation("Get way");

			return Ok(_WayGen.GetLatestWayModel());
        }

		




    [HttpGet("/preload")]
        public ActionResult<List<LoadModel>> Preload()
        {
            _logger.LogInformation("Get load");

			WayModel Temp = _WayGen.GetLatestWayModel();

			List<LoadModel> Load = new List<LoadModel>();

			foreach (Background back in Temp.BackLoad)
			{
				_logger.LogInformation("./wwwroot/" + back.fileName.Replace('\\', '/'));
                LoadModel item = new LoadModel
				{
					sources = new Sources
					{
						webm = new SourcesWebm
						{
							source = back.fileName,
							
							size = (int) new FileInfo("./wwwroot" + back.fileName.Replace('\\','/')).Length
                        }
					}

				};

				Load.Add(item);	
			}

            return Ok(Load);
        }
    }
}
