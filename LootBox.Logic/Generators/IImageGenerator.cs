namespace LootBox.Logic.Generators
{
	/// <summary>
	/// A generator abstraction that handles generating an image.
	/// </summary>
	public interface IImageGenerator
	{
		/// <summary>
		/// Gets a byte array which represents an image of the provided
		/// <paramref name="width"/> and <paramref name="height"/>.
		/// </summary>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		/// <returns>
		/// A <see langword="byte"/> array containing image data.
		/// </returns>
		byte[] GetBytes(int width, int height);

		/// <summary>
		/// Gets a byte array which represents a random image of the provided
		/// <paramref name="width"/> and <paramref name="height"/>.
		/// </summary>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		/// <returns>
		/// A <see langword="byte"/> array containing random image data.
		/// </returns>
		byte[] GetRandomBytes(int width, int height);

		/// <summary>
		/// Saves a <see langword="byte"/> array as an image, using the provided
		/// <paramref name="filename"/>.
		/// </summary>
		/// <param name="bytes">
		/// A <see langword="byte"/> array containing image data.
		/// </param>
		/// <param name="filename">
		/// The file name of the image to save.
		/// </param>
		/// <param name="width">
		/// The width of the image.
		/// </param>
		/// <param name="height">
		/// The height of the image.
		/// </param>
		void Save(byte[] bytes, string filename, int width, int height);
	}
}
