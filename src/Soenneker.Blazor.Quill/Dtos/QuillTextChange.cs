using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Quill.Dtos;

/// <summary>
/// Data raised when the editor contents change.
/// </summary>
public sealed class QuillTextChange
{
    /// <summary>
    /// Gets or sets source.
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets html.
    /// </summary>
    [JsonPropertyName("html")]
    public string? Html { get; set; }

    /// <summary>
    /// Gets or sets text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets contents json.
    /// </summary>
    [JsonPropertyName("contentsJson")]
    public string? ContentsJson { get; set; }

    /// <summary>
    /// Gets or sets delta json.
    /// </summary>
    [JsonPropertyName("deltaJson")]
    public string? DeltaJson { get; set; }

    /// <summary>
    /// Gets or sets old delta json.
    /// </summary>
    [JsonPropertyName("oldDeltaJson")]
    public string? OldDeltaJson { get; set; }
}
