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
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the Quill is ready for use.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);
}
