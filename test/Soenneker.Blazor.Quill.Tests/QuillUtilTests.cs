using System;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.JSInterop;
using Soenneker.Blazor.Quill.Abstract;
using Soenneker.Blazor.Quill.Dtos;
using Soenneker.Blazor.Quill.Options;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Quill.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class QuillUtilTests : HostedUnitTest
{
    private readonly IQuillUtil _quillUtil;

    public QuillUtilTests(Host host) : base(host)
    {
        _quillUtil = Resolve<IQuillUtil>(true);
    }

    [Test]
    public void QuillUtil_ShouldResolveFromContainer()
    {
        _quillUtil.Should().NotBeNull();
        _quillUtil.Should().BeOfType<QuillUtil>();
    }

    [Test]
    public void Constructor_ShouldThrow_WhenInteropIsNull()
    {
        Action act = () => _ = new QuillUtil(null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("interop");
    }

    [Test]
    public async ValueTask Initialize_ShouldDelegateToInterop()
    {
        var interop = new TrackingQuillInterop();
        var util = new QuillUtil(interop);
        using var cancellationTokenSource = new CancellationTokenSource();

        await util.Initialize(cancellationTokenSource.Token);

        interop.InitializeCallCount.Should().Be(1);
        interop.LastCancellationToken.Should().Be(cancellationTokenSource.Token);
    }

    private sealed class TrackingQuillInterop : IQuillInterop
    {
        public int InitializeCallCount { get; private set; }

        public CancellationToken LastCancellationToken { get; private set; }

        public ValueTask Initialize(CancellationToken cancellationToken = default)
        {
            InitializeCallCount++;
            LastCancellationToken = cancellationToken;
            return ValueTask.CompletedTask;
        }

        public ValueTask Create(string elementId, DotNetObjectReference<QuillEventBridge> dotNetReference, QuillOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask<string?> GetHtml(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask SetHtml(string elementId, string? html, string source = "api", CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask<string> GetText(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask SetText(string elementId, string? text, string source = "api", CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask<string> GetContents(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask SetContents(string elementId, string contentsJson, string source = "api", CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask Enable(string elementId, bool enabled = true, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask Focus(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask Blur(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask<QuillSelectionRange?> GetSelection(string elementId, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask SetSelection(string elementId, int index, int length = 0, string source = "api", CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
