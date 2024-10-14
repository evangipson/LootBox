using LootBox.Domain.Models;

namespace LootBox.Logic.Factories
{
	/// <summary>
	/// A factory that is responsible for creating <see cref="Item"/> models.
	/// </summary>
	public interface IItemFactory
	{
		/// <summary>
		/// Creates an <see cref="Item"/> based on the provided parameters.
		/// </summary>
		/// <param name="level">
		/// The level of the <see cref="Item"/>.
		/// </param>
		/// <param name="rarity">
		/// The <see cref="ItemRarity"/> of the <see cref="Item"/>.
		/// </param>
		/// <param name="itemType">
		/// The <see cref="ItemType"/> of the <see cref="Item"/>.
		/// </param>
		/// <returns>
		/// A newly-created <see cref="Item"/>, with the provided parameters factored in.
		/// </returns>
		Item CreateItem(int level, ItemRarity rarity = ItemRarity.Common, ItemType itemType = ItemType.Potion);
	}
}
