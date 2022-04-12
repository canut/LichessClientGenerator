# Lichess Client Generator

The goal of this project was to play with [NSwag C# Client generator](https://github.com/RicoSuter/NSwag/wiki/CSharpClientGenerator) to automatically generate a client API to interact with [Lichess API](https://lichess.org/api).

## Usage

Using the generator is very simple and only requires 2 commands.

```bash
dotnet build
dotnet run swagger.json outputDir/LichessAPIClient.cs
```

You can then copy the generated code where you need it.
