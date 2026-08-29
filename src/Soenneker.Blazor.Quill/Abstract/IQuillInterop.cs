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
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the Quill is ready for use.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a Quill editor for the specified element.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="dotNetReference">JavaScript-invokable reference to the .NET component instance.</param>
    /// <param name="options">Options to configure for the quill.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the create operation is complete.</returns>
    ValueTask Create(string elementId, DotNetObjectReference<QuillEventBridge> dotNetReference, QuillOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys the editor instance for the specified element.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the destroy operation is complete.</returns>
    ValueTask Destroy(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current editor HTML.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by get HTML.</returns>
    ValueTask<string?> GetHtml(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the editor HTML.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="html">Rendered page HTML to inspect.</param>
    /// <param name="source">source to read or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the html has been stored.</returns>
    ValueTask SetHtml(string elementId, string? html, string source = "api", CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current plain text value.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by get Text.</returns>
    ValueTask<string> GetText(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the current plain text value.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="text">Text to read, write, or transform.</param>
    /// <param name="source">source to read or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the text has been stored.</returns>
    ValueTask SetText(string elementId, string? text, string source = "api", CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current Quill Delta as JSON.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by get Contents.</returns>
    ValueTask<string> GetContents(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the current Quill Delta from JSON.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="contentsJson">Contents JSON for the set contents operation.</param>
    /// <param name="source">source to read or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the contents has been stored.</returns>
    ValueTask SetContents(string elementId, string contentsJson, string source = "api", CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables or disables the editor.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="enabled">Whether enabled.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the enable operation is complete.</returns>
    ValueTask Enable(string elementId, bool enabled = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Focuses the editor.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the focus operation is complete.</returns>
    ValueTask Focus(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes focus from the editor.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the blur operation is complete.</returns>
    ValueTask Blur(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current selection range.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested quill Selection Range.</returns>
    ValueTask<QuillSelectionRange?> GetSelection(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the current selection range.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="index">Zero-based position of the target item.</param>
    /// <param name="length">Length for the set selection operation.</param>
    /// <param name="source">source to read or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the selection has been stored.</returns>
    ValueTask SetSelection(string elementId, int index, int length = 0, string source = "api", CancellationToken cancellationToken = default);
}
