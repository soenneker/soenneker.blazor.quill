using System.Text.Json;
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

    /// <summary>Receives a JavaScript text event using generated JSON metadata.</summary>
    /// <param name="payload">The event payload from JavaScript.</param>
    /// <returns>A task that completes when the callback completes.</returns>
    [JSInvokable("OnTextChanged")]
    public Task OnTextChangedFromJson(JsonElement payload) =>
        OnTextChanged(payload.Deserialize(InteropJsonContext.Default.QuillTextChange)!);

    /// <summary>Invokes the text change callback.</summary>
    /// <param name="change">The editor change.</param>
    /// <returns>A task that completes when the callback completes.</returns>
    public Task OnTextChanged(QuillTextChange change)
    {
        return _onTextChanged.Invoke(change).AsTask();
    }

    /// <summary>Receives a JavaScript selection event using generated JSON metadata.</summary>
    /// <param name="payload">The event payload from JavaScript.</param>
    /// <returns>A task that completes when the callback completes.</returns>
    [JSInvokable("OnSelectionChanged")]
    public Task OnSelectionChangedFromJson(JsonElement payload) =>
        OnSelectionChanged(payload.Deserialize(InteropJsonContext.Default.QuillSelectionChange)!);

    /// <summary>Invokes the selection change callback.</summary>
    /// <param name="change">The editor change.</param>
    /// <returns>A task that completes when the callback completes.</returns>
    public Task OnSelectionChanged(QuillSelectionChange change)
    {
        return _onSelectionChanged.Invoke(change).AsTask();
    }
}
