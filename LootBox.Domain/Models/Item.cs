namespace LootBox.Domain.Models
{
	public class Item
	{
		public int Level { get; set; }

		public string? Name { get; set; }

		public string? Type => TypeId.ToString();

		public string? Rarity => RarityId.ToString();

		public IEnumerable<StatModifier> Modifiers { get; set; } = [];

		public ItemType TypeId { get; set; }

		public ItemRarity RarityId { get; set; }
	}
}
