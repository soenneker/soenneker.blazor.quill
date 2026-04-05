using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Quill.Dtos;

namespace Soenneker.Blazor.Quill;

/// <summary>
/// Bridges Quill editor events back into Blazor callbacks.
/// </summary>
public sealed class QuillEventBridge
{
    private readonly Func<ValueTask> _onReady;
    private readonly Func<QuillTextChange, ValueTask> _onTextChanged;
    private readonly Func<QuillSelectionChange, ValueTask> _onSelectionChanged;

    public QuillEventBridge(Func<ValueTask> onReady, Func<QuillTextChange, ValueTask> onTextChanged, Func<QuillSelectionChange, ValueTask> onSelectionChanged)
    {
        _onReady = onReady;
        _onTextChanged = onTextChanged;
        _onSelectionChanged = onSelectionChanged;
    }

    [JSInvokable]
    public Task OnReady()
    {
        return _onReady.Invoke().AsTask();
    }

    [JSInvokable]
    public Task OnTextChanged(QuillTextChange change)
    {
        return _onTextChanged.Invoke(change).AsTask();
    }

    [JSInvokable]
    public Task OnSelectionChanged(QuillSelectionChange change)
    {
        return _onSelectionChanged.Invoke(change).AsTask();
    }
}
