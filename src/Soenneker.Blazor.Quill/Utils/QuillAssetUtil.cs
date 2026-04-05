namespace Soenneker.Blazor.Quill.Utils;

internal static class QuillAssetUtil
{
    private const string _cdnScriptPath = "https://cdn.jsdelivr.net/npm/quill@2.0.3/dist/quill.min.js";
    private const string _localScriptPath = "_content/Soenneker.Blazor.Quill/js/quill.min.js";
    private const string _cdnSnowStylePath = "https://cdn.jsdelivr.net/npm/quill@2.0.3/dist/quill.snow.css";
    private const string _cdnBubbleStylePath = "https://cdn.jsdelivr.net/npm/quill@2.0.3/dist/quill.bubble.css";
    private const string _localSnowStylePath = "_content/Soenneker.Blazor.Quill/css/quill.snow.css";
    private const string _localBubbleStylePath = "_content/Soenneker.Blazor.Quill/css/quill.bubble.css";

    public static string GetScriptPath(bool useCdn = true)
    {
        return useCdn ? _cdnScriptPath : _localScriptPath;
    }

    public static string? GetStylePath(string? theme, bool useCdn = true)
    {
        if (string.IsNullOrWhiteSpace(theme))
            return null;

        return theme.ToLowerInvariant() switch
        {
            "snow" => useCdn ? _cdnSnowStylePath : _localSnowStylePath,
            "bubble" => useCdn ? _cdnBubbleStylePath : _localBubbleStylePath,
            _ => null
        };
    }
}
