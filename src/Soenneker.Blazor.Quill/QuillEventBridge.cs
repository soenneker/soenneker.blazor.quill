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

    /// <summary>
    /// Executes the on ready operation.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable]
    public Task OnReady()
    {
        return _onReady.Invoke().AsTask();
    }

    /// <summary>
    /// Executes the on text changed operation.
    /// </summary>
    /// <param name="change">The change.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable]
    public Task OnTextChanged(QuillTextChange change)
    {
        return _onTextChanged.Invoke(change).AsTask();
    }

    /// <summary>
    /// Executes the on selection changed operation.
    /// </summary>
    /// <param name="change">The change.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable]
    public Task OnSelectionChanged(QuillSelectionChange change)
    {
        return _onSelectionChanged.Invoke(change).AsTask();
    }
}
