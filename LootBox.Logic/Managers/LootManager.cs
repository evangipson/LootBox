using LootBox.Domain.Models;
using LootBox.Logic.Factories;
using LootBox.Logic.Generators;

namespace LootBox.Logic.Managers
{
	/// <inheritdoc cref="ILootManager"/>
	public class LootManager(IItemFactory itemFactory, ITextToImageGenerator textToImageGenerator) : ILootManager
	{
		public Item GetLoot(int level = 1) => itemFactory.CreateItem(level);

		public void GenerateLootImage(string fileName, Item? item = null) => textToImageGenerator.GenerateImage(fileName, item?.Name ?? string.Empty);
	}
}
