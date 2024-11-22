namespace train.Model
{


	public enum Landscape
	{
		FOREST,
		FIELD,
		SUBURB,
		STATION,
		MONUMENT,
		TRAIN,
		ANY
	}

	public enum Weather
	{
		RAIN,
		SUN,
		SNOW,
		FOG,
		ANY
	}
	public enum Time
	{
		MORNING,
		MIDDAY,
		AFTERNOON,
		EVENING,
		NIGHT,
		ANY
	}
	public enum OutEffects
	{
		WIND,
		TOWN,
		THUNDER,
		TALK,
		STEPS,
		ANY
	}

	public class Background
	{
		public int Id { get; set; }
		public string fileName { get; set; }
		public Landscape Landscape { get; set; }
		public Weather Weather { get; set; }
		public Time Time { get; set; }
		public OutEffects OutEffects { get; set; }


	}

	class BackgroundEqualityComparer : IEqualityComparer<Background>
	{
		public bool Equals(Background? b1, Background? b2)
		{
			if (ReferenceEquals(b1, b2))
				return true;

			if (b2 is null || b1 is null)
				return false;

			return b1.Id == b2.Id;
		}

		public int GetHashCode(Background b) => b.Id;
	}

	public class Sound
	{
		public int Id { get; set; }
		public string fileNAme { get; set; }
		public Landscape Landscape { get; set; }
		public Weather Weather { get; set; }
		public Time Time { get; set; }
		public OutEffects OutEffects { get; set; }
	}

	public class Station
	{
		public int Id { get; set; }
		public int seed { get; set; }

		public string name { get; set; }
		public string townName { get; set; }
    }




	
	public class LoadModel
	{
		public string type { get; set; } = "VIDEO";
	public Sources sources { get; set; }
	}
	
	public class Sources
	{
		public SourcesWebm webm { get; set; }
	}
	
	public class SourcesWebm
	{
		public string source { get; set; }
		public int size { get; set; }
	}
}
