namespace LootBox.Logic.Generators
{
	/// <summary>
	/// A generator abstraction that handles text-to-speech functionality.
	/// </summary>
	public interface ITextToImageGenerator
	{
		/// <summary>
		/// Generates an image based on the provided <paramref name="prompt"/>, and
		/// saves it using the provided <paramref name="fileName"/>.
		/// </summary>
		/// <param name="prompt">
		/// An optional prompt for the image.
		/// </param>
		/// <param name="width">
		/// The width for the generated image.
		/// </param>
		/// <param name="height">
		/// The height for the generated image.
		/// </param>
		byte[] GenerateImage(string? prompt = null, int width = 200, int height = 400);
	}
}
