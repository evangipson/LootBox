using LootBox.Logic.Factories;
using LootBox.Logic.Generators;
using LootBox.Logic.Managers;
using Microsoft.Extensions.FileProviders;

namespace LootBox.Api.Extensions
{
	public static class BootstrapExtensions
	{
		public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
		{
			builder.Services
				.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()))
				.AddScoped<IImageGenerator, PngGenerator>()
				.AddScoped<ITextToImageGenerator, DiffusionImageGenerator>()
				.AddScoped<IItemFactory, ItemFactory>()
				.AddScoped<ILootManager, LootManager>()
				.AddControllers();

			return builder;
		}

		public static WebApplication ConfigureApplication(this WebApplication app)
		{
			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
			app.UseStaticFiles();

			return app;
		}
	}
}
