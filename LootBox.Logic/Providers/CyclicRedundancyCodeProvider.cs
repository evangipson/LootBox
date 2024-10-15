namespace LootBox.Logic.Providers
{
	/// <summary>
	/// A provider that abstracts creating cyclic redundancy codes used in PNG image encoding.
	/// </summary>
	public class CyclicRedundancyCodeProvider
	{
		/// <summary>
		/// A <see langword="private"/> backing field for <see cref="Table"/>.
		/// </summary>
		private static uint[]? _table;

		/// <summary>
		/// A table that is used during <see langword="byte"/> redundancy code calculation.
		/// </summary>
		private static uint[] Table => _table ??= CreateTable();

		/// <summary>
		/// Calculates a CRC-32 (Cyclic Redundancy Code) for <paramref name="bytes"/> of a PNG image.
		/// </summary>
		public static byte[] GetRedundancyBytes(byte[] bytes)
		{
			uint crc = 0xffffffff;
			for (int i = 0; i < bytes.Length; ++i)
			{
				byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
				crc = (crc >> 8) ^ Table[index];
			}

			return BitConverter.GetBytes(~crc).Reverse().ToArray();
		}

		/// <summary>
		/// Creates a table that is used during <see langword="byte"/> redundancy calculation.
		/// </summary>
		/// <returns>
		/// A <see langword="uint"/> array to use during <see langword="byte"/> redundancy calculation.
		/// </returns>
		private static uint[] CreateTable()
		{
			uint[] table = new uint[256];
			uint poly = 0xedb88320;
			uint temp;
			for (uint i = 0; i < table.Length; ++i)
			{
				temp = i;
				for (int j = 8; j > 0; --j)
				{
					if ((temp & 1) == 1)
					{
						temp = (temp >> 1) ^ poly;
						continue;
					}
					temp >>= 1;
				}
				table[i] = temp;
			}

			return table;
		}
	}
}
