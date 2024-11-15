using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using train.Model;

namespace train.Service
{
	public class ResourcesContext:DbContext
	{
		public ResourcesContext(DbContextOptions<ResourcesContext> options)
: base(options) { }
		public DbSet<Background> Backgrounds { get; set; }
		public DbSet<Sound> Sounds { get; set; }
		public DbSet<Station> Stations { get; set; }
	}
}
