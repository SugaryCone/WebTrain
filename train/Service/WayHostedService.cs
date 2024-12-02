using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using train.Model;
using train.Servise;

namespace train.Service
{

	public class WayHostedService : BackgroundService
	{
		private readonly IServiceProvider _provider;
		private TimeSpan _refreshInterval = TimeSpan.FromSeconds(100);
		private readonly ILogger<WayHostedService> _logger;
		private readonly WayGen _wayGen;

		public WayHostedService(
			IServiceProvider provider,
			ILogger<WayHostedService> logger,
			WayGen wayGen)
		{
			_wayGen = wayGen;
			_provider = provider;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Fetching latest rates");
				WayModel way = await _wayGen.RunAsync();

				_logger.LogInformation("sss" + way.Backgrounds.ToString());

				_logger.LogInformation("sss" + way.BackgroundsSampleCount.ToString());

				foreach (Background b in way.Backgrounds)
				{
					_logger.LogInformation("sss" + b.fileName);
				}
				_logger.LogWarning(way.Duration.ToString());
                _refreshInterval = TimeSpan.FromSeconds(way.Duration/1000);
                //_refreshInterval = TimeSpan.FromSeconds(10);

				_logger.LogInformation(_refreshInterval.ToString());

				_logger.LogInformation("Latest rates updated");

				await Task.Delay(_refreshInterval, stoppingToken);
			}
		}
	}
}
