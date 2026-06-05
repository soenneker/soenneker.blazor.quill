using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Quill.Dtos;

/// <summary>
/// Represents a Quill selection range.
/// </summary>
public sealed class QuillSelectionRange
{
    /// <summary>
    /// Gets or sets index.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets length.
    /// </summary>
    [JsonPropertyName("length")]
    public int Length { get; set; }
}
