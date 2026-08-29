[![](https://img.shields.io/nuget/v/soenneker.blazor.quill.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.quill/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.quill/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.quill/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.quill.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.quill/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.quill)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.quill/codeql.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.quill/actions/workflows/codeql.yml)

# Soenneker.Blazor.Quill

A Blazor component and interop API for the Quill rich-text editor, with two-way HTML, plain-text, or Delta JSON state and typed editor events.

## Installation

```bash
dotnet add package Soenneker.Blazor.Quill
```

```csharp
using Soenneker.Blazor.Quill.Registrars;

builder.Services.AddQuillInteropAsScoped();
```

The component loads Quill's script and the selected theme stylesheet on demand; do not add duplicate Quill assets to the page.

## Editor with HTML state

```razor
@using Soenneker.Blazor.Quill
@using Soenneker.Blazor.Quill.Dtos
@using Soenneker.Blazor.Quill.Options

<QuillEditor @ref="_editor"
             Options="_options"
             @bind-Html="_html"
             OnTextChange="HandleTextChange"
             style="min-height: 16rem;" />

@code {
    private QuillEditor? _editor;
    private string? _html = "<p>Start typing…</p>";

    private readonly QuillOptions _options = new()
    {
        Placeholder = "Write a description",
        Theme = "snow",
        Modules = new Dictionary<string, object?>
        {
            ["toolbar"] = new object[]
            {
                new[] { "bold", "italic", "underline" },
                new[] { "link", "blockquote" },
                new[] { "clean" }
            }
        }
    };

    private Task HandleTextChange(QuillTextChange change)
    {
        // change.Source is normally "user", "api", or "silent".
        // change also contains Text, ContentsJson, DeltaJson, and OldDeltaJson.
        return Task.CompletedTask;
    }
}
```

Bind one canonical representation—`Html`, `Text`, or `ContentsJson`—rather than binding all three back into the editor. Every text-change event still exposes all representations when secondary values are needed.

Delta JSON preserves Quill's document model and is usually the best choice when content will return to Quill. HTML is convenient for display and interoperability; plain text discards formatting. `GetText()` includes Quill's terminating newline for a non-empty document.

## Programmatic control

The editor is created after its first render. Call methods from `OnReady`, a user event, or a later lifecycle stage:

```csharp
await _editor!.SetText("Draft");
await _editor.Focus();
await _editor.SetSelection(index: 0, length: 5);

string? html = await _editor.GetHtml();
string deltaJson = await _editor.GetContents();

await _editor.Enable(false);
await _editor.Clear();
```

Set `ManualCreate = true` in `QuillOptions` when creation must be deferred, then call `_editor.Create(options)` after the component has rendered. Methods called before creation throw `InvalidOperationException`.

## HTML and upload safety

`SetHtml` uses Quill's `dangerouslyPasteHTML` API. Treat HTML loaded into or read from the editor as untrusted: sanitize it with an allowlist appropriate to the eventual renderer, both before storage and/or before rendering. Do not render stored editor output as raw markup without that boundary.

Adding `image` or `video` to a toolbar enables Quill's default embed behavior; it does not implement a secure application upload pipeline. Provide a controlled handler that validates size and content on the server, generates storage names, and inserts only the returned trusted URL. Consider restricting `Formats` so persisted content cannot contain formats the application does not support.

## Assets and preload

`UseCdn` defaults to `true` and loads pinned Quill assets from jsDelivr. Set it to `false` for the packaged script and `snow`/`bubble` styles. The scoped loader initializes its script source once, so use a consistent source choice for all editors in a scope.

To preload default Quill assets without rendering an editor:

```csharp
builder.Services.AddQuillUtilAsScoped();

// Inject IQuillUtil, then call after JavaScript interop is available.
await Quill.Initialize();
```
