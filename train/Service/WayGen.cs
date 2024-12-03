using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using train.Model;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace train.Servise
{
	public class WayGen
	{
		private readonly Random RandomStation = new Random();
		//private readonly IBackgrounds backgrounds;
		private readonly IServiceProvider _provider;
		private readonly ILogger<WayGen> _logger;
        private readonly IOptions<WayGenSettings> _settings;
		private WayModel? _way;



		static int ii= 0;



        public WayModel? GetLatestWayModel() => _way;

		public WayGen(IServiceProvider provider, ILogger<WayGen> logger, IOptions<WayGenSettings> settings)
		{
			_provider = provider;
			_logger = logger;
            _settings = settings;
		}


        //НУЖНО ИСПРАВИТЬ
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
				Time = Model.Time.AFTERNOON,
			};

            string str_forest = "\\train\\forest.webm";
            string str_field = "\\train\\field.webm";

            string str_suburbINfirst = "\\train\\trainSUBwebm\\sub70-40.webm";
            string str_suburbOUTfirst = "\\train\\trainSUBwebm\\sub20-40.webm";
            string str_suburbINsecond = "\\train\\trainSUBwebm\\sub40-20.webm";
            string str_suburbOUTsecond = "\\train\\trainSUBwebm\\sub40-70.webm";
            string str_suburb = "\\train\\forest.webm";

            string str_monument = "\\train\\canyon.webm";
            string str_train = "\\train\\canyon.webm";

            if (L == Landscape.FOREST)
            {
                B.fileName = str_forest;
                B.Id = 1;
            }

            if (L == Landscape.FIELD)
            {
                B.fileName = str_field;
                B.Id = 2;
            }

            if (L == Landscape.SUBURBinFIRST)
            {
                B.fileName = str_suburbINfirst;
                B.Id = 3;
            }
            if (L == Landscape.SUBURBoutFIRST)
            {
                B.fileName = str_suburbOUTfirst;
                B.Id = 4;
            }

            if (L == Landscape.SUBURBinSECOND)
            {
                B.fileName = str_suburbINsecond;
                B.Id = 5;
            }

            if (L == Landscape.SUBURBoutSECOND)
            {
                B.fileName = str_suburbOUTsecond;
                B.Id = 6;
            }

            if (L == Landscape.SUBURB)
            {
                B.fileName = str_suburb;
                B.Id = 7;
            }

            if (L == Landscape.MONUMENT)
            {
                B.fileName = str_monument;

                B.Id = 8;
            }

            if (L == Landscape.TRAIN)
            {
                B.fileName = str_train;

                B.Id = 9;
            }

            _logger.LogInformation(B.fileName);
			return B;
		}

        // ниже описана функция получения кол-ва видео-фрагментов
        private int videoNum(Random Generator)
        {
            int value = Generator.Next(_settings.Value.minWayDur, _settings.Value.maxWayDur);
            int videoNum = value / _settings.Value.videoDuration; 

            return videoNum;
        }

        //Генератор
        public List<Landscape> generator(int ASeed, int DSeed)
        {
            Random Generator = new Random(ASeed + DSeed);

            List<Landscape> outList = new List<Landscape>();
            List<int> video = new List<int>();

            int rnd_buff = 0;

            int video_id = 0;

            int N_v = videoNum(Generator);

            _logger.LogDebug("\n\tCOUNT OF LANDSCAPES: " + N_v.ToString() + "\n");

            int monument_id = Generator.Next(_settings.Value.leftMonumentPos, N_v - _settings.Value.rightMonumentPos);

            _logger.LogDebug("\n\tMONUMENT ID: " + monument_id.ToString() + "\n");

            int N_suburb = Generator.Next(_settings.Value.minSuburbNumber, _settings.Value.maxSuburbNumber);

            int N_empty = N_v - N_suburb * 2;

            for (int i = 0; i < 3; i++)
            {
                int N_video = Generator.Next((N_empty / 4) - 3, (N_empty / 4) + 3);//Знать бы что тут творится
                video.Add(N_video);
            }
            video.Add(N_empty - (video[0] + video[1] + video[2]));

            outList.Add(Landscape.SUBURBoutFIRST);
            outList.Add(Landscape.SUBURBoutSECOND);

            for (int i = 0; i < N_suburb-2; i++)
            {
                outList.Add(Landscape.SUBURB);
            }

            for (int i = 0; i < 4; i++)
            {

                Landscape landscape = Landscape.ANY;

                if (i == 1)
                    rnd_buff = Math.Abs(rnd_buff - 1);
                else
                    rnd_buff = Generator.Next(0, 2);

                if (rnd_buff == 0)
                    landscape = Landscape.FOREST;
                else
                    landscape = Landscape.FIELD;

                for (int j = 0; j < video[i]; j++)
                {
                    video_id++;
                    if (monument_id == video_id)
                        outList.Add(Landscape.MONUMENT);
                    else
                        outList.Add(landscape);
                }
            }

            for (int i = 0; i < N_suburb-2; i++)
            {
                outList.Add(Landscape.SUBURB);
            }

            outList.Add(Landscape.SUBURBinFIRST);
            outList.Add(Landscape.SUBURBinSECOND);

            int max = -1;
            int idMax = 0;

            for (int i = 0; i < video.Count(); i++)
            {
                if(max < video[i])
                {
                    max = video[i];
                    idMax = i;
                }

            }

            int idTrain = N_suburb;

            _logger.LogDebug("------------------" + idMax.ToString());

            for (int k = 0; k < video.Count(); k++)
            {

                if (k< idMax)
                    idTrain += video[k];
                else
                {
                    idTrain += video[k] / 2;
                    break;
                }

            }

            outList[idTrain] = Landscape.TRAIN;

            for (int i = 0; i < outList.Count(); i++)
                _logger.LogDebug(outList[i].ToString());

            return outList;
        }

        static public Tuple<List<Landscape>,List<int>> listsFull(List<Landscape> _landscapes)
        {
            List<Landscape> landscapes = new List<Landscape>();
            List<int> countLandscapes = new List<int>();

            Tuple<List<Landscape>, List<int>> outTuple = new Tuple<List<Landscape>, List<int>>(landscapes, countLandscapes);

            Landscape currentLandscape = _landscapes[0];
            landscapes.Add(currentLandscape);
            countLandscapes.Add(0);

            int j = 0;

            for (int i = 0; i < _landscapes.Count(); i++)
            {

                if (currentLandscape == _landscapes[i])
                {
                    countLandscapes[j]++;
                }
                else
                {
                    currentLandscape = _landscapes[i];
                    landscapes.Add(currentLandscape);
                    j++;
                    countLandscapes.Add(0);
                    countLandscapes[j]++;
                }

            }

            return outTuple;
        }

        public void loggerFunc(Tuple<List<Landscape>, List<int>> _way)
        {
            _logger.LogInformation("\n\tThere is log information: \n");
            _logger.LogInformation("\nList and count of landscapes: ");
            for (int i = 0; i < _way.Item1.Count(); ++i)
                _logger.LogInformation(_way.Item1[i].ToString() + _way.Item2[i].ToString());
        }
        public async Task<int> RunAsync()
		{
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


            //Сет уникальных фрагментов
            HashSet<Background> loadSet = new HashSet<Background>(new BackgroundEqualityComparer());
            //Список с конечной генерацией
            List<Background> outList = new List<Background>();
            //Список кол-ва видео-фрагментов в четвертях
            List<int> video = new List<int>();

            Background nextBack = null;

            List<Landscape> wayGen = generator(A.seed, D.seed);

            Tuple<List<Landscape>, List<int>> wayTuple = listsFull(wayGen);

            if (wayTuple.Item1.Count() == wayTuple.Item2.Count())
            {
                for (int i = 0; i < wayTuple.Item1.Count(); ++i)
                {
                    nextBack = await GetBackground(wayTuple.Item1[i], Weather.SUN, Time.AFTERNOON, OutEffects.ANY);
                    outList.Add(nextBack);
                    loadSet.Add(nextBack);
                    video.Add(wayTuple.Item2[i]);
                }
            }

            int durationLandscape = 0;

            for (int i = 0; i < wayTuple.Item2.Count(); ++i)
            {
                durationLandscape += wayTuple.Item2[i];
            }

            loggerFunc(wayTuple);

            durationLandscape = durationLandscape * _settings.Value.videoDuration *1000;//in milliseconds

            WayModel way = new WayModel
            {
                DepartureTime = DateTime.Now.AddMilliseconds(_settings.Value.wayGap).ToUniversalTime(),
                Departure = D,
                Arrival = A,
                Duration = durationLandscape,
                BackLoad = loadSet.ToList(),
                Backgrounds = outList,
                BackgroundsSampleCount = video,
                Sounds = new List<Sound>()
            };

            _way = way;
            return way.Duration;
        }

    }
}