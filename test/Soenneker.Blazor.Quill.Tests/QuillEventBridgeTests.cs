using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Blazor.Quill.Dtos;

namespace Soenneker.Blazor.Quill.Tests;

public sealed class QuillEventBridgeTests
{
    [Test]
    public async Task OnReady_ShouldInvokeCallback()
    {
        var called = false;
        var bridge = new QuillEventBridge(() =>
        {
            called = true;
            return ValueTask.CompletedTask;
        }, _ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        await bridge.OnReady();

        called.Should().BeTrue();
    }

    [Test]
    public async Task OnTextChanged_ShouldPassChangeToCallback()
    {
        QuillTextChange? received = null;
        var expected = new QuillTextChange
        {
            Html = "<p>Hello</p>",
            Text = "Hello",
            ContentsJson = """{"ops":[{"insert":"Hello\n"}]}""",
            Source = "user"
        };

        var bridge = new QuillEventBridge(() => ValueTask.CompletedTask, change =>
        {
            received = change;
            return ValueTask.CompletedTask;
        }, _ => ValueTask.CompletedTask);

        await bridge.OnTextChanged(expected);

        received.Should().BeSameAs(expected);
    }

    [Test]
    public async Task OnSelectionChanged_ShouldPassChangeToCallback()
    {
        QuillSelectionChange? received = null;
        var expected = new QuillSelectionChange
        {
            Range = new QuillSelectionRange { Index = 2, Length = 4 },
            OldRange = new QuillSelectionRange { Index = 0, Length = 1 },
            Source = "user"
        };

        var bridge = new QuillEventBridge(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask, change =>
        {
            received = change;
            return ValueTask.CompletedTask;
        });

        await bridge.OnSelectionChanged(expected);

        received.Should().BeSameAs(expected);
    }
}
