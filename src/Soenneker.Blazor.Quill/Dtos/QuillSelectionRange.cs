using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Quill.Dtos;

/// <summary>
/// Represents a Quill selection range.
/// </summary>
public sealed class QuillSelectionRange
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }
}
