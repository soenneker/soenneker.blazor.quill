[![](https://img.shields.io/nuget/v/soenneker.blazor.quill.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.quill/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.quill/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.quill/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.quill.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.quill/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.quill)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.quill/codeql.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.quill/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Quill
### A Blazor interop library for Quill, the editor

## Installation

```bash
dotnet add package Soenneker.Blazor.Quill
```

## Setup

Register services in `Program.cs`:

```csharp
builder.Services.AddQuillInteropAsScoped();
```

The component lazy-loads the Quill assets for you, so you do not need to add the Quill CSS or script tags manually.

## Usage

```razor
@using Soenneker.Blazor.Quill
@using Soenneker.Blazor.Quill.Dtos
@using Soenneker.Blazor.Quill.Options

<QuillEditor @ref="_editor"
             Options="_options"
             @bind-Html="_html"
             @bind-Text="_text"
             OnTextChange="OnTextChange"
             OnSelectionChange="OnSelectionChange" />

@code {
    private QuillEditor? _editor;
    private string? _html;
    private string? _text;

    private readonly QuillOptions _options = new()
    {
        Placeholder = "Start typing...",
        Theme = "snow",
        Modules = new Dictionary<string, object?>
        {
            ["toolbar"] = new object[]
            {
                new[] {"bold", "italic", "underline"},
                new[] {"link", "image"},
                new[] {"clean"}
            }
        }
    };

    private Task OnTextChange(QuillTextChange change)
    {
        Console.WriteLine(change.Html);
        return Task.CompletedTask;
    }

    private Task OnSelectionChange(QuillSelectionChange change)
    {
        Console.WriteLine(change.Range?.Index);
        return Task.CompletedTask;
    }
}
```

## Programmatic API

Use the component reference when you want to control Quill from Blazor code:

```csharp
await _editor!.SetHtml("<p>Hello from Blazor</p>");
string html = await _editor.GetHtml() ?? "";
string text = await _editor.GetText();
string deltaJson = await _editor.GetContents();

await _editor.Focus();
await _editor.SetSelection(0, 5);
await _editor.Clear();
```

## Optional Manual Initialization

If you only want to preload the package resources without rendering the component yet, the existing utility is still available:

```csharp
builder.Services.AddQuillUtilAsScoped();

@inject IQuillUtil Quill

await Quill.Initialize();
```
