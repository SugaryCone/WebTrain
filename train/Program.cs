using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.WebEncoders;
using Npgsql;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using train.Service;
using train.Servise;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();



builder.Services.Configure<WebEncoderOptions>(options =>
{
	options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Creating a CORS policy

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		builder => builder.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader());
});

builder.Services.AddDbContext<ResourcesContext>(opt =>
	opt.UseNpgsql(builder.Configuration["Base"]));


builder.Services.AddTransient<IBackgrounds, BackgroundsService>();
builder.Services.AddSingleton<WayGen>();
builder.Services.AddHostedService<WayHostedService>();





var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.MapSwagger().RequireAuthorization();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.UseStaticFiles();


app.UseRouting();
app.UseAntiforgery();


app.Run();




/*app.UseCors("AllowAll");


if (!app.Environment.IsDevelopment())
{
	app.UseSwagger()
   .UseSwaggerUI(c =>
   {
	   c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
   });
	app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
*/