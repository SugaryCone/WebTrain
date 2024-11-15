namespace train.Model
{
	public class WayModel
	{
		public DateTime DepartureTime { get; set; }
		public Station Departure { get; set; }
		public Station Arrival { get; set; }

		public int Duration { get; set; }

		public ICollection<Background> BackLoad { get; set; } 
		public ICollection<Background> Backgrounds { get; set; }
		public ICollection<int> BackgroundsSampleCount { get; set; }
		public ICollection<Sound> Sounds { get; set; }



//Equals(Object)
	}
}
									