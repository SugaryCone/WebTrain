namespace train.Model
{
	public class WayGenSettings
    {
		
		public int durationTrain {  get; set; }
		public int wayGap { get; set; }
		public int videoDuration {  get; set; }
		public int minWayDur { get; set; }
		public int maxWayDur { get; set; }
		public int minSuburbNumber { get; set; }
		public int maxSuburbNumber { get; set; }
		public int leftMonumentPos { get; set; }
		public int rightMonumentPos { get; set; }

	}
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
									