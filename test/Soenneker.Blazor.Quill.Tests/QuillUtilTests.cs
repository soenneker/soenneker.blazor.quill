using Soenneker.Blazor.Quill.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Quill.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class QuillUtilTests : HostedUnitTest
{
    private readonly IQuillUtil _blazorlibrary;

    public QuillUtilTests(Host host) : base(host)
    {
        _blazorlibrary = Resolve<IQuillUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
