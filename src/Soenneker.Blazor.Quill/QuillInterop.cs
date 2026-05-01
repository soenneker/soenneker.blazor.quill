using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Quill.Abstract;
using Soenneker.Blazor.Quill.Dtos;
using Soenneker.Blazor.Quill.Options;
using Soenneker.Blazor.Quill.Utils;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.Quill;

/// <inheritdoc cref="IQuillInterop"/>
public sealed class QuillInterop : IQuillInterop
{
    private const string _modulePath = "_content/Soenneker.Blazor.Quill/js/quillinterop.js";
    private const string _quillVariable = "Quill";

    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly AsyncInitializer<bool> _scriptInitializer;
    private readonly AsyncInitializer _moduleInitializer;
    private readonly HashSet<string> _loadedStyles = [];
    private readonly SemaphoreSlim _styleSemaphore = new(1, 1);
    private readonly CancellationScope _cancellationScope = new();

    public QuillInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;
        _scriptInitializer = new AsyncInitializer<bool>(InitializeScript);
        _moduleInitializer = new AsyncInitializer(InitializeModule);
    }

    private async ValueTask InitializeScript(bool useCdn, CancellationToken cancellationToken)
    {
        string scriptPath = QuillAssetUtil.GetScriptPath(useCdn);
        await _resourceLoader.LoadScriptAndWaitForVariable(scriptPath, _quillVariable, cancellationToken: cancellationToken);
    }

    private async ValueTask InitializeModule(CancellationToken cancellationToken)
    {
        _ = await _moduleImportUtil.GetContentModuleReference(_modulePath, cancellationToken);
    }

    private async ValueTask EnsureStyleLoaded(string? theme, bool useCdn, CancellationToken cancellationToken)
    {
        string? stylePath = QuillAssetUtil.GetStylePath(theme, useCdn);

        if (stylePath == null)
            return;

        await _styleSemaphore.WaitAsync(cancellationToken);

        try
        {
            if (_loadedStyles.Contains(stylePath))
                return;

            await _resourceLoader.LoadStyle(stylePath, cancellationToken: cancellationToken);
            _loadedStyles.Add(stylePath);
        }
        finally
        {
            _styleSemaphore.Release();
        }
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            var options = new QuillOptions();

            await _scriptInitializer.Init(options.UseCdn, linked);
            await _moduleInitializer.Init(linked);
            await EnsureStyleLoaded(options.Theme, options.UseCdn, linked);
        }
    }

    public async ValueTask Create(string elementId, DotNetObjectReference<QuillEventBridge> dotNetReference, QuillOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            options ??= new QuillOptions();

            await _scriptInitializer.Init(options.UseCdn, linked);
            await _moduleInitializer.Init(linked);
            await EnsureStyleLoaded(options.Theme, options.UseCdn, linked);

            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("create", linked, elementId, dotNetReference, options);
        }
    }

    public async ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("destroy", linked, elementId);
        }
    }

    public async ValueTask<string?> GetHtml(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            return await module.InvokeAsync<string?>("getHtml", linked, elementId);
        }
    }

    public async ValueTask SetHtml(string elementId, string? html, string source = "api", CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? cancellationSource);

        using (cancellationSource)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setHtml", linked, elementId, html, source);
        }
    }

    public async ValueTask<string> GetText(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            return await module.InvokeAsync<string>("getText", linked, elementId);
        }
    }

    public async ValueTask SetText(string elementId, string? text, string source = "api", CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? cancellationSource);

        using (cancellationSource)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setText", linked, elementId, text, source);
        }
    }

    public async ValueTask<string> GetContents(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            return await module.InvokeAsync<string>("getContents", linked, elementId);
        }
    }

    public async ValueTask SetContents(string elementId, string contentsJson, string source = "api", CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? cancellationSource);

        using (cancellationSource)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setContents", linked, elementId, contentsJson, source);
        }
    }

    public async ValueTask Enable(string elementId, bool enabled = true, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("enable", linked, elementId, enabled);
        }
    }

    public async ValueTask Focus(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("focus", linked, elementId);
        }
    }

    public async ValueTask Blur(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("blur", linked, elementId);
        }
    }

    public async ValueTask<QuillSelectionRange?> GetSelection(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            return await module.InvokeAsync<QuillSelectionRange?>("getSelection", linked, elementId);
        }
    }

    public async ValueTask SetSelection(string elementId, int index, int length = 0, string source = "api", CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? cancellationSource);

        using (cancellationSource)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setSelection", linked, elementId, index, length, source);
        }
    }

    public async ValueTask DisposeAsync()
    {
        _styleSemaphore.Dispose();
        await _moduleImportUtil.DisposeContentModule(_modulePath);
        await _scriptInitializer.DisposeAsync();
        await _moduleInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
