using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Quill.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Quill.Registrars;

/// <summary>
/// Registration for the interop and utility services.
/// </summary>
public static class QuillUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IQuillInterop"/> as a scoped service.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddQuillInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped()
                .TryAddScoped<IQuillInterop, QuillInterop>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IQuillInterop"/> and <see cref="IQuillUtil"/> as scoped services.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddQuillUtilAsScoped(this IServiceCollection services)
    {
        services.AddQuillInteropAsScoped();
        services.TryAddScoped<IQuillUtil, QuillUtil>();

        return services;
    }
}
