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
    /// Responds when ready occurs.
    /// </summary>
    /// <returns>A task that completes when the on ready operation is complete.</returns>
    [JSInvokable]
    public Task OnReady()
    {
        return _onReady.Invoke().AsTask();
    }

    /// <summary>
    /// Responds when text changed occurs.
    /// </summary>
    /// <param name="change">Change for the on text changed operation.</param>
    /// <returns>A task that completes when the on text changed operation is complete.</returns>
    [JSInvokable]
    public Task OnTextChanged(QuillTextChange change)
    {
        return _onTextChanged.Invoke(change).AsTask();
    }

    /// <summary>
    /// Responds when selection changed occurs.
    /// </summary>
    /// <param name="change">Change for the on selection changed operation.</param>
    /// <returns>A task that completes when the on selection changed operation is complete.</returns>
    [JSInvokable]
    public Task OnSelectionChanged(QuillSelectionChange change)
    {
        return _onSelectionChanged.Invoke(change).AsTask();
    }
}
