using System.Collections.Generic;

namespace Soenneker.Blazor.Quill.Options;

/// <summary>
/// Configuration used to create a Quill editor instance.
/// </summary>
public sealed class QuillOptions
{
    /// <summary>
    /// If true, Quill assets are loaded from CDN. If false, packaged local assets are used.
    /// </summary>
    public bool UseCdn { get; set; } = true;

    /// <summary>
    /// Quill theme. Common values are <c>snow</c>, <c>bubble</c>, or <c>null</c> for core styling only.
    /// </summary>
    public string? Theme { get; set; } = "snow";

    /// <summary>
    /// Placeholder text shown when the editor is empty.
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// If true, the editor is initialized in read-only mode.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Optional selector or element boundary used by Quill for tooltips and popups.
    /// </summary>
    public string? Bounds { get; set; }

    /// <summary>
    /// Optional Quill debug level such as <c>error</c>, <c>warn</c>, <c>log</c>, or <c>false</c>.
    /// </summary>
    public string? Debug { get; set; }

    /// <summary>
    /// Optional whitelist of supported formats.
    /// </summary>
    public List<string>? Formats { get; set; }

    /// <summary>
    /// Optional Quill modules configuration, such as toolbar/history/clipboard.
    /// </summary>
    public Dictionary<string, object?>? Modules { get; set; }

    /// <summary>
    /// If true, the component will not automatically create the editor on first render.
    /// </summary>
    public bool ManualCreate { get; set; }
}
