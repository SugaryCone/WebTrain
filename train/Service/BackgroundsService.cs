using Microsoft.EntityFrameworkCore;
using train.Model;
using train.Service;

namespace train.Servise
{

	public interface IBackgrounds
	{
		public Background GetBackOn(Landscape L, Weather W, Time T, OutEffects O);
		public void SetBack(Background back);
	}

	public class BackgroundsService:IBackgrounds
	{
		private ResourcesContext _backgrounds;
		private static Random _random = new Random(DateTime.Now.Second);
		public BackgroundsService(ResourcesContext backgrounds)
		{
			_backgrounds = backgrounds;
		}


		public Background GetBackOn(Landscape L, Weather W, Time T, OutEffects O)
		{
			List<Background> Backs = _backgrounds.Backgrounds.Where(b => b.Landscape == L &&
												b.Weather == W &&
												b.Time == T &&
												b.OutEffects == O).ToList<Background>();

			Background result = new Background();

			if (Backs.Count == 1) {
				result	= Backs[0];
			}
			else if (Backs.Count > 1)
			{
				result = Backs[_random.Next(Backs.Count)];
			}


			return result;

		}

		public void SetBack(Background back)
		{
			_backgrounds.Backgrounds.Add(back);
			_backgrounds.SaveChanges();
		}

	}
}
