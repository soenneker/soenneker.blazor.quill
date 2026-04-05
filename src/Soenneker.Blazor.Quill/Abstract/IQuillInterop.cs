using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Blazor.Quill.Dtos;
using Soenneker.Blazor.Quill.Options;

namespace Soenneker.Blazor.Quill.Abstract;

/// <summary>
/// Blazor interop for browser-facing functionality exposed by this package.
/// </summary>
public interface IQuillInterop : IAsyncDisposable
{
    /// <summary>
    /// Ensures the JavaScript resources for this package have been loaded and initialized.
    /// </summary>
    ValueTask Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a Quill editor for the specified element.
    /// </summary>
    ValueTask Create(string elementId, DotNetObjectReference<QuillEventBridge> dotNetReference, QuillOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys the editor instance for the specified element.
    /// </summary>
    ValueTask Destroy(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current editor HTML.
    /// </summary>
    ValueTask<string?> GetHtml(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the editor HTML.
    /// </summary>
    ValueTask SetHtml(string elementId, string? html, string source = "api", CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current plain text value.
    /// </summary>
    ValueTask<string> GetText(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the current plain text value.
    /// </summary>
    ValueTask SetText(string elementId, string? text, string source = "api", CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current Quill Delta as JSON.
    /// </summary>
    ValueTask<string> GetContents(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the current Quill Delta from JSON.
    /// </summary>
    ValueTask SetContents(string elementId, string contentsJson, string source = "api", CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables or disables the editor.
    /// </summary>
    ValueTask Enable(string elementId, bool enabled = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Focuses the editor.
    /// </summary>
    ValueTask Focus(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes focus from the editor.
    /// </summary>
    ValueTask Blur(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current selection range.
    /// </summary>
    ValueTask<QuillSelectionRange?> GetSelection(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the current selection range.
    /// </summary>
    ValueTask SetSelection(string elementId, int index, int length = 0, string source = "api", CancellationToken cancellationToken = default);
}
