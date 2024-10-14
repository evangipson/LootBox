namespace LootBox.Logic.Generators
{
	/// <summary>
	/// An implementation of <see cref="IImageGenerator"/> for PNG.
	/// </summary>
	public class PngGenerator : IImageGenerator
	{
		private readonly Random _random = new();

		private const byte _ihdrHeaderLength = 13;
		private const byte _bitDepth = 8;
		private const byte _colorType = 2;
		private const byte _compressionMethod = 0;
		private const byte _filterMethod = 0;
		private const byte _interlaceMethod = 0;

		private uint[]? _crcTable;
		private uint[] CrcTable
		{
			get
			{
				if (_crcTable != null)
				{
					return _crcTable;
				}

				_crcTable = new uint[256];
				uint poly = 0xedb88320;
				uint temp;

				for (uint i = 0; i < _crcTable.Length; ++i)
				{
					temp = i;
					for (int j = 8; j > 0; --j)
					{
						if ((temp & 1) == 1)
						{
							temp = (uint)((temp >> 1) ^ poly);
						}
						else
						{
							temp >>= 1;
						}
					}
					_crcTable[i] = temp;
				}

				return _crcTable;
			}
		}

		private static readonly FileStreamOptions _fileStreamOptions = new()
		{
			Mode = FileMode.OpenOrCreate & FileMode.Truncate,
			Access = FileAccess.Write
		};

		private static readonly Dictionary<string, byte[]> _pngChunks = new()
		{
			/* a signature common to all png files */
			["Signature"] = [137, 80, 78, 71, 13, 10, 26, 10],

			/* 4-byte length of IHDR chunk */
			["IHDRLength"] = [0, 0, 0, _ihdrHeaderLength],
			/* 4-byte header chunk common to all png files */
			["IHDRHead"] = [73, 72, 68, 82],
			/* 5-byte PNG information for the tail of the IHDR chunk */
			["IHDRTail"] = [_bitDepth, _colorType, _compressionMethod, _filterMethod, _interlaceMethod],

			/* a 4-byte signature to begin image data for png files */
			["IDAT"] = [73, 68, 65, 84],

			/* 4-byte ending signature common to all png files */
			["IENDLength"] = [0, 0, 0, 0],
			/* 4-byte ending signature common to all png files */
			["IEND"] = [73, 69, 78, 68],
		};

		public byte[] GetBytes(int width, int height)
		{
			throw new NotSupportedException($"{nameof(GetBytes)} is not supported by {nameof(PngGenerator)}. Use {nameof(GetRandomBytes)} instead.");
		}

		public byte[] GetRandomBytes(int width, int height)
		{
			var totalBytes = new byte[width * height];
			return totalBytes.SelectMany(@byte => new byte[]
			{
				(byte)_random.Next(0, 255),
				(byte)_random.Next(0, 255),
				(byte)_random.Next(0, 255)
			}).ToArray();
        }

		public void Save(byte[] bytes, string filename, int width, int height)
		{
			using FileStream fileStream = new(filename, _fileStreamOptions);

			fileStream.Write(_pngChunks["Signature"], 0, _pngChunks["Signature"].Length);

			WritePngHeader(fileStream, width, height);
			WritePngFile(fileStream, bytes);
			WritePngFooter(fileStream);

			fileStream.Flush();
		}

		/// <summary>
		/// Writes a required PNG header chunk (IHDR) to the provided <paramref name="fileStream"/>.
		/// </summary>
		/// <param name="fileStream">
		/// A <see cref="FileStream"/> that is writing PNG data.
		/// </param>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		private void WritePngHeader(FileStream fileStream, int width, int height)
		{
			fileStream.Write(_pngChunks["IHDRLength"], 0, _pngChunks["IHDRLength"].Length);

			WriteToPngWithCyclicRedundancy(fileStream,
			[
				.. _pngChunks["IHDRHead"],
				.. BitConverter.GetBytes(width).Reverse().ToArray(),
				.. BitConverter.GetBytes(height).Reverse().ToArray(),
				.. _pngChunks["IHDRTail"]
			]);
		}

		/// <summary>
		/// Writes a <see langword="byte"/> array containing RGB tuples to the provided <paramref name="fileStream"/>.
		/// </summary>
		/// <param name="fileStream">
		/// A <see cref="FileStream"/> that is writing PNG data.
		/// </param>
		/// <param name="rgbBytes">
		/// A <see langword="byte"/> array containing RGB tuples.
		/// </param>
		private void WritePngFile(FileStream fileStream, byte[] rgbBytes)
		{
			var idatLength = BitConverter.GetBytes(rgbBytes.Length).Reverse().ToArray();
			fileStream.Write(idatLength, 0, idatLength.Length);

			WriteToPngWithCyclicRedundancy(fileStream,
			[
				.. _pngChunks["IDAT"],
				.. rgbBytes
			]);
		}

		/// <summary>
		/// Writes a required PNG footer chunk (IEND) to the provided <paramref name="fileStream"/>.
		/// </summary>
		/// <param name="fileStream">
		/// A <see cref="FileStream"/> that is writing PNG data.
		/// </param>
		private void WritePngFooter(FileStream fileStream)
		{
			fileStream.Write(_pngChunks["IENDLength"], 0, _pngChunks["IENDLength"].Length);
			WriteToPngWithCyclicRedundancy(fileStream, _pngChunks["IEND"]);
		}

		/// <summary>
		/// Writes the <paramref name="pngBytes"/> to the <paramref name="fileStream"/>.
		/// <para>
		/// Also writes a CRC32 code after <paramref name="pngBytes"/>. The CRC32 must
		/// <b>not</b> include the length section of the chunk.
		/// </para>
		/// </summary>
		/// <param name="fileStream">
		/// A <see cref="FileStream"/> that is writing PNG data.
		/// </param>
		/// <param name="pngBytes">
		/// A  <see langword="byte"/> array containing image data.
		/// </param>
		private void WriteToPngWithCyclicRedundancy(FileStream fileStream, byte[] pngBytes)
		{
			fileStream.Write(pngBytes, 0, pngBytes.Length);
			WriteCyclicRedundancyCheck(fileStream, pngBytes);
		}


		/// <summary>
		/// Calculates a CRC32 (Cyclic Redundancy Code) for <paramref name="bytes"/>
		/// of a PNG image, then writes that calculated CRC32 to the <paramref name="fileStream"/>.
		/// </summary>
		public void WriteCyclicRedundancyCheck(FileStream fileStream, byte[] bytes)
		{
			uint crc = 0xffffffff;
			for (int i = 0; i < bytes.Length; ++i)
			{
				byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
				crc = (crc >> 8) ^ CrcTable[index];
			}

			fileStream.Write(BitConverter.GetBytes(~crc).Reverse().ToArray(), 0, 4);
		}
	}
}
