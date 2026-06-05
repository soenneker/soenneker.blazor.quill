using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Quill.Dtos;

/// <summary>
/// Data raised when the editor selection changes.
/// </summary>
public sealed class QuillSelectionChange
{
    /// <summary>
    /// Gets or sets range.
    /// </summary>
    [JsonPropertyName("range")]
    public QuillSelectionRange? Range { get; set; }

    /// <summary>
    /// Gets or sets old range.
    /// </summary>
    [JsonPropertyName("oldRange")]
    public QuillSelectionRange? OldRange { get; set; }

    /// <summary>
    /// Gets or sets source.
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }
}
