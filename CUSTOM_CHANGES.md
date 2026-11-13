# JavaScript Injector (HAC): custom changes

AXL's hardened fork of [n00bcodr/Jellyfin-JavaScript-Injector](https://github.com/n00bcodr/Jellyfin-JavaScript-Injector), GPL-3.0. Nothing here goes back upstream.

## Branch model

- `main` mirrors upstream `main` exactly (fast-forward only); never commit to it.
- `custom` = an upstream release tag + small, separate commits, one per change. Today the base is tag `4.1.0.0` = `98de5d2`.
- The `upstream` remote's push URL is `DISABLED`.
- Releases are built only by `.github/workflows/build.yml` on GitHub's runners, from a tag `v<version>-hac` on `custom`.

## Change table

| Change | Commit | Upstream files touched | Why |
|---|---|---|---|
| Local scripts only, no plugin registration, no disk writes | `8947e22` | `Controllers/CustomJavaScriptController.cs`, `Services/JavaScriptRegistrationService.cs`, `PluginInterface.cs` (rewritten as refusing stubs), `Configuration/PluginConfiguration.cs`, `Configuration/configPage.html`, `Services/StartupService.cs`, `Plugin.cs`; deleted `Helpers/TransformationPatches.cs`, `Model/PatchRequestPayload.cs` | Only scripts an admin enters run, only from this server, and the plugin never edits Jellyfin's files |
| Own identity, version 4.1.0.1, Jellyfin 12.4.0 | `bb17d4a` | `Plugin.cs` (GUID, name), `configPage.html` (GUID, help link), `.csproj` (version, 12.4.0 packages), `README.md` (notice); deleted `manifest.json`; new `packaging/meta.json` | A new GUID means no upstream catalogue entry can update it; `autoUpdate` false; built for the server it runs on |
| Build workflow, no Newtonsoft | this commit | `.csproj` (Newtonsoft.Json reference removed), `PluginInterface.cs` (`object` parameter); new `.github/workflows/build.yml`, `CUSTOM_CHANGES.md` | Reproducible builds on GitHub's runners with pinned actions and least privilege; the zip holds only our DLL and `meta.json` |

The plugin id is `559ef066-e27f-4961-bd06-8ea1bdfc924c`, and its name in Jellyfin is "JavaScript Injector (HAC)".

## Rebasing onto a new upstream release

1. Fast-forward `main` from `upstream/main` and fetch the new tag.
2. Rebase `custom` onto the new tag.
3. Expect conflicts in:
   - `CustomJavaScriptController.cs`, `PluginInterface.cs`, `StartupService.cs`, `configPage.html`: keep our behaviour;
   - `.csproj`: keep our version line and the Jellyfin 12.4 package version, raised to the server's version.
4. Raise the version in `.csproj` and `packaging/meta.json` together (for example 4.2.0.1), then tag `v4.2.0.1-hac`.
5. Re-check the review findings (`docs/custom/jsinjector-review.md` in the HolyArrs Fork project) against the new upstream code before tagging.
