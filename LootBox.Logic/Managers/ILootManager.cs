using LootBox.Domain.Models;

namespace LootBox.Logic.Managers
{
	/// <summary>
	/// A manager that is responsible for getting <see cref="Item"/> models.
	/// </summary>
	public interface ILootManager
	{
		/// <summary>
		/// Gets an <see cref="Item"/> based on the provided <paramref name="level"/>.
		/// </summary>
		/// <param name="level">
		/// The level of the generated <see cref="Item"/>, defaults to <c>1</c>.
		/// </param>
		/// <returns>
		/// A new <see cref="Item"/> based on the provided <paramref name="level"/>.
		/// </returns>
		Item GetLoot(int level = 1);

		/// <summary>
		/// Generates an image for the provided <paramref name="item"/>.
		/// </summary>
		/// <param name="item">
		/// An optional <see cref="Item"/> to base the generated image on.
		/// </param>
		byte[] GenerateLootImage(Item? item = null);
	}
}
