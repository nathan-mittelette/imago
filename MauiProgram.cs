using CommunityToolkit.Maui;
using imago.Services;
using imago.Utils;
using imago.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

namespace imago;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        EntryHandler.Mapper.AppendToMapping("EntryBorder", (handler, view) =>
        {
            handler.PlatformView.Layer.BorderWidth = 1; // Set the border width
            handler.PlatformView.Layer.BorderColor =
                CoreGraphics.CGColor.CreateSrgb(0.7f, 0.7f, 0.7f, 1); // Fixed color (gray)
            handler.PlatformView.Layer.CornerRadius = 5; // Optional: Add rounded corners
        });

        PickerHandler.Mapper.AppendToMapping("PickerBorder", (handler, view) =>
        {
            handler.PlatformView.Layer.BorderWidth = 1; // Set the border width
            handler.PlatformView.Layer.BorderColor =
                CoreGraphics.CGColor.CreateSrgb(0.7f, 0.7f, 0.7f, 1); // Fixed color (gray)
            handler.PlatformView.Layer.CornerRadius = 5; // Optional: Add rounded corners
        });

        // ViewModels
        builder.Services.AddSingleton<ImageViewModel>();
        builder.Services.AddSingleton<ResizeViewModels>();

        // Services
        builder.Services.AddSingleton<GlobalStorageService>();
        builder.Services.AddSingleton<ToastService>();
        builder.Services.AddSingleton<ImageConverterService>();
        builder.Services.AddSingleton<ImageLoaderService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        ServiceLocator.Instance = app.Services;
        return app;
    }
}