using LootBox.Api.Extensions;

namespace LootBox.Api
{
	internal class Program
	{
		internal static void Main() => WebApplication.CreateBuilder()
			.ConfigureBuilder()
			.Build()
			.ConfigureApplication()
			.Run();
	}
}
