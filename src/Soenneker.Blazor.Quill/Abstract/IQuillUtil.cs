using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Quill.Abstract;

/// <summary>
/// A higher-level Blazor utility built on top of <see cref="IQuillInterop"/>.
/// </summary>
public interface IQuillUtil
{
    /// <summary>
    /// Ensures the underlying JavaScript module has been loaded and is ready for use.
    /// </summary>
    ValueTask Initialize(CancellationToken cancellationToken = default);
}
