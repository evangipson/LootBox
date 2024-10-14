using LootBox.Domain.Models;

namespace LootBox.Logic.Factories
{
	/// <inheritdoc cref="IItemFactory"/>
	public class ItemFactory : IItemFactory
	{
		public Item CreateItem(int level, ItemRarity rarity = ItemRarity.Common, ItemType itemType = ItemType.Potion)
		{
			return new()
			{
				Name = "New Item",
				Level = level,
				RarityId = rarity,
				TypeId = itemType
			};
		}
	}
}
