# LootBox
A .NET web api that generates loot using generated imagery.

# Development
- Clone the repository
- Open in Visual Studio
- Set startup project to `LootBox.Api`
- Run the project

# Concepts
The following concepts are meant to be illustrated in this project.

## Fluent Bootstrapping
`LootBox.Api/Program.cs` contains an example of fluent bootstrapping, using the `LootBox.Api/Extensions/BootstrapExtensions.cs` extension methods to encapsulate bootstrap behavior.

## Clean Architecture
`LootBox.Domain` has no dependencies, and purely contains models for the application.

`LootBox.Logic` is only dependent on `LootBox.Domain`, and works with the models contained in `LootBox.Domain` to generate meaningful data.

`LootBox.Api` is the application layer, and dependent on both `LootBox.Logic` and `LootBox.Domain`.

Dependencies are only one-way, which mitigates potential circular references, and forces the developer to write code in the proper layer.

## Extensible Abstractions
There are a collection of interfaces for image generation, along with an implementation for each of them.

### Text Prompt to Image Generation
The `ITextToImageGenerator` interface contains abstractions useful for creating images from text prompts.

The `DiffusionImageGenerator` is an implementation of `ITextToImageGenerator` that uses a diffusion process (de-noising a random noise image) to create images.

### Image Encoding
The `IImageGenerator` interface contains abstractions useful for generating images from byte arrays.

The `PngGenerator` is an implementation of `IImageGenerator` that will encode a byte array containing RGB data (pixels) to the PNG format.

# Roadmap
- [x] Scaffold basic loot generation at the `/loot` endpoint
- [x] Scaffold abstractions for text-to-image generation and image generation
- [ ] Scaffold implementations of text-to-image and image generation abstractions
- [ ] Scaffold system for persisting generated loot, more than likely using caching or POST body
- [ ] Scaffold system for applying modifiers to persisted generated loot