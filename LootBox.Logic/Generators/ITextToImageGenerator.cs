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
		/// <param name="fileName">
		/// The file name for the generated image.
		/// </param>
		/// <param name="prompt">
		/// A prompt for the image.
		/// </param>
		/// <param name="width">
		/// The width for the generated image.
		/// </param>
		/// <param name="height">
		/// The height for the generated image.
		/// </param>
		void GenerateImage(string fileName, string prompt, int width = 24, int height = 24);
	}
}
