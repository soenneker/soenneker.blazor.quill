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
}
