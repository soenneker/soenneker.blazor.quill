using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Quill.Abstract;

namespace Soenneker.Blazor.Quill;

/// <inheritdoc cref="IQuillUtil"/>
public sealed class QuillUtil : IQuillUtil
{
    private readonly IQuillInterop _interop;

    public QuillUtil(IQuillInterop interop)
    {
        _interop = interop ?? throw new ArgumentNullException(nameof(interop));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        return _interop.Initialize(cancellationToken);
    }
}
