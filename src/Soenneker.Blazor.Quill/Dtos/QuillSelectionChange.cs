using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Quill.Dtos;

/// <summary>
/// Data raised when the editor selection changes.
/// </summary>
public sealed class QuillSelectionChange
{
    [JsonPropertyName("range")]
    public QuillSelectionRange? Range { get; set; }

    [JsonPropertyName("oldRange")]
    public QuillSelectionRange? OldRange { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }
}
