using LootBox.Logic.Providers;
using static LootBox.Domain.Constants.PngConstants;

namespace LootBox.Logic.Generators
{
	/// <summary>
	/// An implementation of <see cref="IImageGenerator"/> for generating PNG images.
	/// </summary>
	public class PngGenerator : IImageGenerator
	{
		private readonly Random _random = new();

		public byte[] GetBytes(int width, int height) => throw new NotSupportedException($"{nameof(GetBytes)} is not supported by {nameof(PngGenerator)}.");

		public byte[] GetRandomBytes(int width, int height) => new byte[width * height * BytesPerPixel]
			.Select(@byte => (byte)_random.Next(255))
			.ToArray();

		public byte[] Save(byte[] bytes, int width, int height)
		{
			using MemoryStream memoryStream = new();

			WritePngHeader(memoryStream, width, height);
			WritePngFile(memoryStream, bytes, width, height);
			WritePngFooter(memoryStream);

			return memoryStream.ToArray();
		}

		/// <summary>
		/// Writes the <see cref="ImageChunks.Signature"/> and <see cref="ImageChunks.IHDR"/>
		/// <see cref="ImageChunk"/> bytes to the provided <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">
		/// A <see cref="Stream"/> that is writing PNG data.
		/// </param>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		private static void WritePngHeader(Stream stream, int width, int height)
		{
			byte[] headerBytes = [
				.. ImageChunks.IHDR.SignatureBytes,
				.. BitConverter.GetBytes(width).Reverse().ToArray(),
				.. BitConverter.GetBytes(height).Reverse().ToArray(),
				.. ImageChunks.DefaultIHDRConfiguration
			];

			stream.Write(ImageChunks.Signature, 0, ImageChunks.Signature.Length);
			stream.Write(ImageChunks.IHDR.LengthBytes, 0, ImageChunks.IHDR.LengthBytes.Length);
			WriteToPngWithCyclicRedundancy(stream, headerBytes);
		}

		/// <summary>
		/// Writes the <see cref="ImageChunks.IDAT"/> <see cref="ImageChunk"/> and a <see cref="Pixel"/>
		/// representation of <paramref name="rgbBytes"/> to the provided <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">
		/// A <see cref="Stream"/> that writes <paramref name="rgbBytes"/> as <see cref="Pixel"/> bytes.
		/// </param>
		/// <param name="rgbBytes">
		/// A <see langword="byte"/> array containing R, G, B byte triples.
		/// </param>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		private static void WritePngFile(Stream stream, byte[] rgbBytes, int width, int height)
		{
			var pixelData = GetPixelBytes(rgbBytes, width, height);
			var idatLength = BitConverter.GetBytes(pixelData.Length).Reverse().ToArray();
			byte[] pngFileBytes =
			[
				.. ImageChunks.IDAT.SignatureBytes,
				.. pixelData
			];

			stream.Write(idatLength, 0, idatLength.Length);
			stream.Write(pngFileBytes, 0, pngFileBytes.Length);
			WriteCyclicRedundancyCheck(stream, pngFileBytes);
		}

		/// <summary>
		/// Writes a required PNG footer chunk (IEND) to the provided <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream">
		/// A <see cref="Stream"/> that is writing PNG data.
		/// </param>
		private static void WritePngFooter(Stream stream)
		{
			stream.Write(ImageChunks.IEND.LengthBytes, 0, ImageChunks.IEND.LengthBytes.Length);
			WriteToPngWithCyclicRedundancy(stream, ImageChunks.IEND.SignatureBytes);
		}

		/// <summary>
		/// Writes the <paramref name="pngBytes"/> to the <paramref name="stream"/>.
		/// <para>
		/// Also writes a CRC32 code after <paramref name="pngBytes"/>. The CRC32 must
		/// <b>not</b> include the length section of the chunk.
		/// </para>
		/// </summary>
		/// <param name="stream">
		/// A <see cref="Stream"/> that is writing PNG data.
		/// </param>
		/// <param name="pngBytes">
		/// A  <see langword="byte"/> array containing image data.
		/// </param>
		private static void WriteToPngWithCyclicRedundancy(Stream stream, byte[] pngBytes)
		{
			stream.Write(pngBytes, 0, pngBytes.Length);
			WriteCyclicRedundancyCheck(stream, pngBytes);
		}

		/// <summary>
		/// Calculates a CRC32 (Cyclic Redundancy Code) for <paramref name="bytes"/>
		/// of a PNG image, then writes that calculated CRC32 to the <paramref name="stream"/>.
		/// </summary>
		public static void WriteCyclicRedundancyCheck(Stream stream, byte[] bytes)
		{
			var crcBytes = CyclicRedundancyCodeProvider.GetRedundancyBytes(bytes);
			stream.Write(crcBytes, 0, crcBytes.Length);
		}

		/// <summary>
		/// Gets a byte array of pixels from the provided <paramref name="rgbBytes"/>.
		/// </summary>
		/// <param name="rgbBytes">
		/// A single-dimensional array of R, G, B triples.
		/// </param>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		/// <returns>
		/// A <see langword="new"/> <see langword="byte"/> array with mapped <see cref="Pixel"/> data.
		/// </returns>
		private static byte[] GetPixelBytes(byte[] rgbBytes, int width, int height)
		{
			Pixel pixel;
			int start;
			var buffer = new byte[BytesPerPixel];
			var pixelBytes = new byte[rgbBytes.Length + height];

			using MemoryStream readStream = new(rgbBytes);

			for (int row = 0; row < height; row++)
			{
				for (int column = 0; column < width; column++)
				{
					start = (row * ((width * BytesPerPixel) + 1)) + 1 + (column * BytesPerPixel);

					readStream.Read(buffer, 0, buffer.Length);
					pixel = new()
					{
						R = buffer[0],
						G = buffer[1],
						B = buffer[2]
					};

					pixelBytes[start++] = pixel.R;
					pixelBytes[start++] = pixel.G;
					pixelBytes[start++] = pixel.B;
				}
			}

			return pixelBytes;
		}
	}
}
