using System;
using System.Reflection.Emit;
using train.Model;

namespace train.Servise
{
	public class WayGen
	{
		private readonly Random RandomStation = new Random();
		//private readonly IBackgrounds backgrounds;
		private readonly IServiceProvider _provider;
		private readonly ILogger<WayGen> _logger;
		private WayModel? _way;
		static int ii= 0;
		public WayModel? GetLatestWayModel() => _way;

		public WayGen(IServiceProvider provider,
		ILogger<WayGen> logger)
		{
			_provider = provider;
			_logger = logger;
		}


		public async Task<Background> GetBackground(Landscape L, Weather W, Time T, OutEffects O)
		{
			using var scope = _provider.CreateScope();

			/*IBackgrounds backgroundsClient = scope.ServiceProvider.GetRequiredService<IBackgrounds>();

			Background B = backgroundsClient.GetBackOn(L, W, T, O);*/

			Background B = new Model.Background
			{
				fileName = "\\train\\forest_better.webm",
				Landscape = Model.Landscape.FOREST,
				Weather = Model.Weather.SUN,
				OutEffects = Model.OutEffects.ANY,
				Time = Model.Time.AFTERNOON
			};
			_logger.LogInformation(B.fileName);
			return B;
		}

		public async Task<WayModel> RunAsync()
		{
			_logger.LogInformation("Run");
			//gen stations
			Station D = new Station
			{
				name = "Tuda",
				townName = "Tuda",
				seed = DateTime.Now.Second
			};
			Station A = new Station
			{
				name = "Suda",
				townName = "Suda",
				seed = DateTime.Now.Second/2
            };
			//gen duration
			Random Generator = new Random(D.seed + A.seed);

			int duration = Generator.Next(600, 1200);
			_logger.LogInformation("duration" + duration.ToString());
			int base_dure = 30;

			while(duration % base_dure != 0)
			{
				duration--;
			}

			double start_dur = duration * 0.15;
			double middle_dur = duration - 2*start_dur;
			HashSet<Background> load_set = new HashSet<Background>(new BackgroundEqualityComparer());
			List<Background> list = new List<Background>();
			List<int> listCounts = new List<int>();
			


			int TokenCountStart = (int)(duration / base_dure);
			_logger.LogInformation("start" + TokenCountStart.ToString());
			//Отъезд - пригород 15 %
			while (TokenCountStart > 0) 
			{
				
				Background B = await GetBackground((Landscape)Generator.Next(0,2), Weather.SUN, Time.AFTERNOON, OutEffects.ANY);

				int sample_count = Generator.Next(1, TokenCountStart);
				TokenCountStart -= sample_count;
				_logger.LogInformation("start------------" + TokenCountStart.ToString());
				load_set.Add(B);
				list.Add(B);
				listCounts.Add(sample_count);

			}





			//Дорога - самая длинная часть
			//Крупный объект					70%
			//Дорога - самая длинная часть


/*			int TokenCountMiddle = (int)(middle_dur / base_dure);
			//Отъезд - пригород 15 %
			while (TokenCountMiddle > 0)
			
			{
				Background B = await GetBackground(Landscape.FOREST, Weather.SUN, Time.AFTERNOON, OutEffects.ANY);
				int sample_count = Generator.Next(1,TokenCountMiddle);
				TokenCountMiddle -= sample_count;

				load_set.Add(B);
				list.Add(B);
				listCounts.Add(sample_count);
			}




			//подъезд - пригород 15%

			int TokenCountEnd = (int)(start_dur / base_dure);
			//Отъезд - пригород 15 %
			while (TokenCountEnd > 0)
			{
				Background B = await GetBackground(Landscape.FOREST, Weather.SUN, Time.AFTERNOON, OutEffects.ANY);
				int sample_count = Generator.Next(1,TokenCountEnd);
				TokenCountEnd -= sample_count;

				load_set.Add(B);
				list.Add(B);
				listCounts.Add(sample_count);

			}*/

			//gen token list from seed
			//gen video and audio from token list

			WayModel way = new WayModel
			{
				DepartureTime = DateTime.Now.ToUniversalTime(),
				Departure = D,
				Arrival = A,
				Duration = duration,
				BackLoad = load_set.ToList(),
				Backgrounds = list,
				BackgroundsSampleCount = listCounts,
				Sounds = new List<Sound>()
			};

			_logger.LogInformation(duration.ToString());
			_way = way;
			return way;
		}


	}
}