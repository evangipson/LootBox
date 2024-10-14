namespace LootBox.Domain.Models
{
	public class StatModifier
	{
		public Stat? Stat { get; set; }

		public int Modifier { get; set; }

		public string? Prefix { get; set; }

		public string? Suffix { get; set; }
	}
}
