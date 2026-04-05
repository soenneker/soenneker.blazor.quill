using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Quill.Dtos;

/// <summary>
/// Data raised when the editor contents change.
/// </summary>
public sealed class QuillTextChange
{
    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("html")]
    public string? Html { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("contentsJson")]
    public string? ContentsJson { get; set; }

    [JsonPropertyName("deltaJson")]
    public string? DeltaJson { get; set; }

    [JsonPropertyName("oldDeltaJson")]
    public string? OldDeltaJson { get; set; }
}
