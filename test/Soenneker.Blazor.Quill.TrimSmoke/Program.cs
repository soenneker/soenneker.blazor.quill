using System.Text.Json;
using Soenneker.Blazor.Quill;

var called = false;
var bridge = new QuillEventBridge(() => ValueTask.CompletedTask, change => { Check(change.Text == "hello", "text callback"); return ValueTask.CompletedTask; }, change => { Check(change.Range is { Index: 3, Length: 2 } && change.OldRange is { Index: 1, Length: 0 }, "nested selection callback"); called = true; return ValueTask.CompletedTask; });
using var selection = JsonDocument.Parse("""{"range":{"index":3,"length":2},"oldRange":{"index":1,"length":0},"source":"user"}""");
await bridge.OnSelectionChangedFromJson(selection.RootElement);
using var text = JsonDocument.Parse("""{"text":"hello","source":"user"}""");
await bridge.OnTextChangedFromJson(text.RootElement);
Check(called, "selection callback invoked");
Check(JsonSerializer.Deserialize("null", InteropJsonContext.Default.QuillSelectionRange) is null, "null selection");

Console.WriteLine("Trimmed JSON smoke checks passed.");

static void Check(bool condition, string name)
{
    if (!condition) throw new InvalidOperationException(name);
}
