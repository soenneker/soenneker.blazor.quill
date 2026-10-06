using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Collections.Generic;
using Soenneker.Blazor.Quill.Options;
using Soenneker.Blazor.Quill.Dtos;

namespace Soenneker.Blazor.Quill;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(QuillSelectionRange))]
[JsonSerializable(typeof(QuillTextChange))]
[JsonSerializable(typeof(QuillSelectionChange))]
[JsonSerializable(typeof(QuillOptions))]
[JsonSerializable(typeof(Dictionary<string, object?>))]
[JsonSerializable(typeof(List<object?>))]
[JsonSerializable(typeof(object[]))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(decimal))]
internal partial class InteropJsonContext : JsonSerializerContext
{
    internal static JsonSerializerOptions WithContext(JsonSerializerContext? additionalContext)
    {
        if (additionalContext is null) return Default.Options;
        var options = new JsonSerializerOptions(Default.Options)
        {
            TypeInfoResolver = JsonTypeInfoResolver.Combine(Default, additionalContext)
        };
        options.MakeReadOnly();
        return options;
    }
}
