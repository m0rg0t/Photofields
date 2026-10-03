# Cached photo metadata regressions

Run from the repository root using .NET SDK 10:

```sh
dotnet run --project tests/PortableLogic/PortableLogic.csproj --configuration Release
```

The test project links the actual `MainViewModel`, `ItemViewModel`, and `BindableBase` production source. The original constructor tries to deserialize the complete cached API response as an array and silently discards it. The regression fails against that original source and passes after reading the envelope's `photos`, `current_page` and `total_pages` properties. Missing, null, malformed and incomplete synthetic caches are also covered. Restored metadata does not claim that image downloads completed.

The host uses the real Newtonsoft.Json assembly bundled with .NET SDK 10 via `$(MSBuildSDKsPath)/../Newtonsoft.Json.dll`; JSON parsing is not mocked. This is a test-host implementation, not a replacement for the application's Newtonsoft.Json 4.5.9 binary. The original dependency's runtime compatibility is **not** established by these tests. There are no NuGet package dependencies and package sources are cleared.

Minimal test-only adapters provide an in-memory roaming-settings dictionary, platform attributes and image type declarations. Image IO fails closed; only the constructor runs. No HTTP methods, credentials, real user photos, Windows storage, device operations, image decoding or UI are exercised. Compilation is restricted to C# 5 syntax.

## Historical application limits

The solution `Photofields/MalukahSongs.sln` selects `MalukahSongs/MalukahSongs.csproj`; it and `Photofields.csproj` include the same edited MainViewModel. Both application projects, binaries and package manifests remain unchanged. A full Windows Store/XAML build still needs the matching historical Windows SDK and original references. The portable host cannot validate WinRT behavior or prove compatibility of modern package assets with that target. Existing Dependabot PR #1 is a separate Newtonsoft manifest-only proposal and is not duplicated.

CI uses read-only permissions, commit-pinned actions and the same synthetic test command. No application deployment or platform migration is included.
