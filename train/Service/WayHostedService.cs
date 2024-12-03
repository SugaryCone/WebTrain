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

				_logger.LogInformation("Generate next way for train");
                int duration = await _wayGen.RunAsync();
				
                _refreshInterval = TimeSpan.FromSeconds(duration/1000);
                _logger.LogInformation($"Duration of next way {_refreshInterval}");
                await Task.Delay(_refreshInterval, stoppingToken);
                _logger.LogInformation("New way create");

            }
		}
	}
}
