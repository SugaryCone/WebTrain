using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
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

            string str_forest = "\\train\\forest_better.webm";
            string str_field = "\\train\\field.webm";
            string str_suburban = "\\train\\forest.webm";
            string str_monument = "\\train\\canyon.webm";

            if (L == Landscape.FOREST)
                B.fileName = str_forest;
            if (L == Landscape.FIELD)
                B.fileName = str_field;
            if (L == Landscape.SUBURB)
                B.fileName = str_suburban;
            if (L == Landscape.MONUMENT)
                B.fileName = str_monument;


            _logger.LogInformation(B.fileName);
			return B;
		}

        // ниже описана функция получения кол-ва видео-фрагментов
        static public int videoNum(Random Generator)
        {
            int value = Generator.Next(600, 900);
            int videoNum = value / 30; //30 seconds

            return (videoNum);
        }

        //Генератор
        static public List<string> generator(int ASeed, int DSeed)
        {
            Random Generator = new Random(ASeed + DSeed);

            List<string> outList = new List<string>();
            List<int> video = new List<int>();

            int rnd_buff = 0;

            int video_id = 0;

            int N_v = videoNum(Generator);

/*            _logger.LogInformation("\n\tCOUNT OF LANDSCAPES: " + N_v.ToString() + "\n");*/

            int monument_id = Generator.Next(2, N_v - 3);

            int N_suburb = Generator.Next(2, 6);

            int N_empty = N_v - N_suburb * 2;

            for (int i = 0; i < 3; i++)
            {
                int N_video = Generator.Next((N_empty / 4) - 3, (N_empty / 4) + 3);
                video.Add(N_video);
            }
            video.Add(N_empty - (video[0] + video[1] + video[2]));

            for (int i = 0; i < N_suburb; i++)
            {
                outList.Add("suburb");
            }

            for (int i = 0; i < 4; i++)
            {

                string str = "";

                if (i == 1)
                    rnd_buff = Math.Abs(rnd_buff - 1);
                else
                    rnd_buff = Generator.Next(0, 2);

                if (rnd_buff == 0)
                    str = "forest";
                else
                    str = "field";

                for (int j = 0; j < video[i]; j++)
                {
                    video_id++;
                    if (monument_id == video_id)
                        outList.Add("monument");
                    else
                        outList.Add(str);
                }
            }

            for (int i = 0; i < N_suburb; i++)
            {
                outList.Add("suburb");
            }

/*            for(int i = 0; i < outList.Count(); i++)
                _logger.LogInformation(outList[i].ToString());*/

            return outList;
        }

        static public Tuple<List<string>,List<int>> listsFull(List<string> _landscapes)
        {
            List<string> landscapes = new List<string>();
            List<int> countLandscapes = new List<int>();

            Tuple<List<string>, List<int>> outTuple = new Tuple<List<string>, List<int>>(landscapes, countLandscapes);

/*            for (int i = 0; i < 7; i++)
                countLandscapes.Add(0);*/

            string str = _landscapes[0];
            landscapes.Add(str);
            countLandscapes.Add(0);

            int j = 0;

            for (int i = 0; i < _landscapes.Count(); i++)
            {

                if (str == _landscapes[i])
                {
                    countLandscapes[j]++;
                }
                else
                {
                    str = _landscapes[i];
                    landscapes.Add(str);
                    j++;
                    countLandscapes.Add(0);
                    countLandscapes[j]++;
                }
/*                _logger.LogInformation("\n\n");
                _logger.LogInformation("str: " + str);
                _logger.LogInformation("_lanscapes[i]: " + _landscapes[i]);
                _logger.LogInformation("j: " + j);
                _logger.LogInformation("countLandscapes[j]: " + countLandscapes[j]);
                _logger.LogInformation("\n\n");*/

            }

            return outTuple;
        }

        static public int landToPath(string _way)
        {
            //id фрагментов
            /*
            FOREST = 0,
		    FIELD = 1,
		    SUBURB = 2,
		    STATION = 3,
		    MONUMENT = 4,
            TRAIN = 5,
		    ANY = 6
            */

            /*            string str_forest = "\\train\\forest_better.webm";
                        string str_field = "\\train\\field.webm";
                        string str_suburban = "\\train\\forest.webm";
                        string str_monument = "\\train\\canyon.webm";*/

            //string outString = "";

            int int_forest = 0;
            int int_field = 1;
            int int_suburban = 2;
            int int_monument = 4;

            int outInt = 0;

            if (_way == "suburb")
                outInt = int_suburban;
            if (_way == "forest")
                outInt = int_forest;
            if (_way == "field")
                outInt = int_field;
            if (_way == "monument")
                outInt = int_monument;

            return outInt;
        }

        public void loggerFunc(Tuple<List<string>, List<int>> _way)
        {
            _logger.LogInformation("\n\tThere is log information: \n");
            _logger.LogInformation("\nList and count of landscapes: ");
            for (int i = 0; i < _way.Item1.Count(); ++i)
                _logger.LogInformation(_way.Item1[i].ToString() + _way.Item2[i].ToString());
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


            //Сет уникальных фрагментов
            HashSet<Background> loadSet = new HashSet<Background>(new BackgroundEqualityComparer());
            //Список с конечной генерацией
            List<Background> outList = new List<Background>();
            //Список кол-ва видео-фрагментов в четвертях
            List<int> video = new List<int>();

            Background B = await GetBackground(Landscape.FOREST, Weather.SUN, Time.AFTERNOON, OutEffects.ANY);

            List<string> wayGen = generator(A.seed, D.seed);

            Tuple<List<string>, List<int>> wayTuple = listsFull(wayGen);

            if (wayTuple.Item1.Count() == wayTuple.Item2.Count())
            {
                for (int i = 0; i < wayTuple.Item1.Count(); ++i)
                {
                    B = await GetBackground((Landscape)landToPath(wayTuple.Item1[i]), Weather.SUN, Time.AFTERNOON, OutEffects.ANY);

                    _logger.LogInformation("\t\n" + B.fileName +"\n");
                    _logger.LogInformation(wayTuple.Item1[i] + "\n");
                    outList.Add(B);
                    loadSet.Add(B);
                    video.Add(wayTuple.Item2[i]);
                }
            }

            int duration = 0;

            for (int i = 0; i < wayTuple.Item2.Count(); ++i)
            {
                duration += wayTuple.Item2[i];
            }

            loggerFunc(wayTuple);

            duration = duration * 30000;

            WayModel way = new WayModel
            {
                DepartureTime = DateTime.Now.ToUniversalTime(),
                Departure = D,
                Arrival = A,
                Duration = duration,
                BackLoad = loadSet.ToList(),
                Backgrounds = outList,
                BackgroundsSampleCount = video,
                Sounds = new List<Sound>()
            };

            _way = way;
            return way;
        }


        //return outList;

        //Заполнили loadSet уникальными значениями, пройдясь по всему outList

        /*            for (int i = 0; i < outList.Count; i++)
                    {
                        loadSet.Add(outList[i]);
                    }*/








        /* int duration = Generator.Next(600, 1200);
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
         List<int> listCounts = new List<int>();*/

        /*            int TokenCountStart = (int)(duration / base_dure);
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

                    }*/





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


    }
}