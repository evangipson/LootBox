namespace LootBox.Logic.Generators
{
	/// <summary>
	/// An implementation of <see cref="ITextToImageGenerator"/> that uses diffusion to generate
	/// imagery from a prompt.
	/// </summary>
	public class DiffusionImageGenerator(IImageGenerator imageGenerator) : ITextToImageGenerator
	{
		private readonly int _timesteps = 10;
		private readonly Random _random = new();

		public byte[] GenerateImage(string? prompt = null, int width = 200, int height = 400)
		{
			// Initialize the image with random noise
			var image = imageGenerator.GetRandomBytes(width, height);

			// Diffusion process
			for (int t = 0; t < _timesteps; t++)
			{
				var noise = GenerateNoise(image.Length);
				image = AddNoise(image, noise, t / _timesteps);
			}

			return imageGenerator.Save(image, width, height);
		}

		/// <summary>
		/// Generates noise to modify the image in the diffusion process.
		/// </summary>
		/// <param name="length">
		/// The length of the <see langword="byte"/> array of the image.
		/// </param>
		/// <returns>
		/// A <see langword="byte"/> array containing values which will be applied
		/// to the image during the diffusion process.
		/// </returns>
		private byte[] GenerateNoise(int length) => [.. Enumerable.Range(0, length).Select(index => (byte)_random.Next(-30, 30))];

		/// <summary>
		/// Adds <paramref name="noise"/> to the provided <paramref name="image"/>
		/// <see langword="byte"/> array.
		/// </summary>
		/// <param name="image">
		/// A <see langword="byte"/> array containing image data.
		/// </param>
		/// <param name="noise">
		/// A <see langword="byte"/> array containing values which will be applied
		/// to the image during the diffusion process.
		/// </param>
		/// <param name="noiseLevel">
		/// A modifier value for scaling how much noise is applied each diffusion pass.
		/// </param>
		/// <returns>
		/// A copy of <paramref name="image"/> with <paramref name="noise"/> applied,
		/// taking in <paramref name="noiseLevel"/> into account.
		/// </returns>
		private static byte[] AddNoise(byte[] image, byte[] noise, float noiseLevel)
		{
			var result = new byte[image.Length];

			for (int i = 0; i < image.Length; i++)
			{
				result[i] = (byte)Math.Abs(image[i] + (int)(noise[i] * noiseLevel));
			}

			return result;
		}
	}
}
