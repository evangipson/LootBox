namespace LootBox.Domain.Constants
{
	/// <summary>
	/// A collection of constant values used for PNG images.
	/// </summary>
	public static class PngConstants
	{
		/// <summary>
		/// Single-byte integer giving the number of bits per sample or per palette index (not per pixel).
		/// </summary>
		public enum BitDepth
		{
			/// <summary>
			/// One bit per palette index.
			/// </summary>
			One = 1,

			/// <summary>
			/// Two bits per palette index.
			/// </summary>
			Two = 2,

			/// <summary>
			/// Four bits per palette index.
			/// </summary>
			Four = 4,

			/// <summary>
			/// Eight bit per palette index.
			/// </summary>
			Eight = 8,

			/// <summary>
			/// Sixteen bit per palette index.
			/// </summary>
			Sixteen = 16
		}

		/// <summary>
		/// Single-byte integer that defines the PNG image type.
		/// </summary>
		public enum ColorType
		{
			/// <summary>
			/// Each pixel is a greyscale sample without transparency.
			/// </summary>
			Grayscale = 0,

			/// <summary>
			/// Each pixel is an R,G,B triple.
			/// </summary>
			Truecolor = 2,

			/// <summary>
			/// Each pixel is a palette index; a PLTE chunk is required.
			/// </summary>
			IndexedColor = 3,

			/// <summary>
			/// Each pixel is a greyscale sample followed by an alpha sample.
			/// </summary>
			GrayscaleAlpha = 4,

			/// <summary>
			/// Each pixel is an R,G,B triple followed by an alpha sample.
			/// </summary>
			TruecolorAlpha = 6
		}

		/// <summary>
		/// Single-byte integer that indicates the method used to compress the image data.
		/// </summary>
		public enum CompressionMethod
		{
			/// <summary>
			/// Adaptive filtering with five basic filter types.
			/// </summary>
			None = 0,
		}

		/// <summary>
		/// Single-byte integer that indicates the preprocessing method applied to the image data before compression
		/// </summary>
		public enum FilterMethod
		{
			/// <summary>
			/// Adaptive filtering with five basic filter types.
			/// </summary>
			Adaptive = 0,
		}

		/// <summary>
		/// Single-byte integer that indicates the transmission order of the image data
		/// </summary>
		public enum InterlaceMethod
		{
			/// <summary>
			/// Pixels are extracted sequentially from left to right, and scanlines sequentially from top to bottom.
			/// </summary>
			NoInterlace = 0,

			/// <summary>
			/// Defines seven distinct passes over the image. Each pass transmits a subset of the pixels in the reference image.
			/// The pass in which each pixel is transmitted (numbered from 1 to 7) is defined by replicating the following 8-by-8
			/// pattern over the entire image, starting at the upper left corner.
			/// </summary>
			Adam7 = 1,
		}

		/// <summary>
		/// A three-byte RGB pixel, used when creating a PNG image.
		/// </summary>
		public struct Pixel()
		{
			/// <summary>
			/// The red value of this <see cref="Pixel"/>. Minimum is 0, maximum is 255.
			/// </summary>
			public byte R { get; set; } = 0;

			/// <summary>
			/// The red value of this <see cref="Pixel"/>. Minimum is 0, maximum is 255.
			/// </summary>
			public byte G { get; set; } = 0;

			/// <summary>
			/// The red value of this <see cref="Pixel"/>. Minimum is 0, maximum is 255.
			/// </summary>
			public byte B { get; set; } = 0;
		}

		/// <summary>
		/// A chunk of metadata used in PNG encoding.
		/// </summary>
		public struct ImageChunk()
		{
			/// <summary>
			/// A four-byte integer that always preceeds the <see cref="SignatureBytes"/> for a <see cref="ImageChunk"/>.
			/// <para>
			/// <b>Not</b> included in the cyclic redundancy check for each <see cref="ImageChunk"/>.
			/// </para>
			/// </summary>
			public byte[] LengthBytes = new byte[4];

			/// <summary>
			/// A four-byte integer that contains a constant value that signals the image <see cref="ImageChunk"/> during decoding.
			/// </summary>
			public byte[] SignatureBytes = new byte[4];
		}

		public static class ImageChunks
		{
			/// <summary>
			/// An eight-byte integer that is a PNG signature that each PNG must start with.
			/// </summary>
			public static readonly byte[] Signature = [137, 80, 78, 71, 13, 10, 26, 10];

			/// <summary>
			/// The "IHDR" image header, must immediately follow the <see cref="Signature"/>.
			/// </summary>
			public static readonly ImageChunk IHDR = new()
			{
				LengthBytes = [0, 0, 0, 13],
				SignatureBytes = [73, 72, 68, 82]
			};

			/// <summary>
			/// The "IHDR" image header, must immediately follow the <see cref="Signature"/>.
			/// </summary>
			public static readonly ImageChunk IDAT = new()
			{
				SignatureBytes = [73, 68, 65, 84]
			};

			/// <summary>
			/// The "IHDR" image header, must immediately follow the <see cref="Signature"/>.
			/// </summary>
			public static readonly ImageChunk IEND = new()
			{
				LengthBytes = [0, 0, 0, 0],
				SignatureBytes = [73, 69, 78, 68]
			};

			/// <summary>
			/// A default configuration for the final five bytes of the IHDR header.
			/// </summary>
			public static readonly byte[] DefaultIHDRConfiguration =
			[
				(byte)BitDepth.Eight,
				(byte)ColorType.Truecolor,
				(byte)CompressionMethod.None,
				(byte)FilterMethod.Adaptive,
				(byte)InterlaceMethod.NoInterlace
			];
		}

		/// <summary>
		/// The amount of <see langword="byte"/> for each <see cref="Pixel"/>.
		/// </summary>
		public const int BytesPerPixel = 3;

		/// <summary>
		/// A default configuration of <see cref="System.IO.FileStreamOptions"/> for image creation.
		/// </summary>
		public static readonly FileStreamOptions FileStreamOptions = new()
		{
			Mode = FileMode.OpenOrCreate & FileMode.Truncate,
			Access = FileAccess.ReadWrite,
			Share = FileShare.ReadWrite
		};
	}
}
