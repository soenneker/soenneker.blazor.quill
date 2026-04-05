using Soenneker.Blazor.Quill.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Quill.Tests;

[Collection("Collection")]
public sealed class QuillUtilTests : FixturedUnitTest
{
    private readonly IQuillUtil _blazorlibrary;

    public QuillUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _blazorlibrary = Resolve<IQuillUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
