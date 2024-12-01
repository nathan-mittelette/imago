using imago.Services;
using imago.Utils;
using imago.ViewModels;

namespace imago;

public partial class App : Application
{
    private readonly ImageLoaderService _imageLoaderService;
    private readonly ImageViewModel _imageViewModel;

    private readonly MainPage _mainPage;

    public App(ImageLoaderService imageLoaderService, ImageViewModel imageViewModel)
    {
        InitializeComponent();
        _imageLoaderService = imageLoaderService;
        _imageViewModel = imageViewModel;
        _mainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(_mainPage);

        window.HandlerChanged += (sender, args) =>
        {
            if (_mainPage.Handler?.MauiContext != null)
            {
                _mainPage.RegisterDrop(_mainPage.Handler.MauiContext,
                    async filePath =>
                    {
                        _imageViewModel.ResetValues();
                        await _imageLoaderService.LoadImage(filePath);
                    });
            }
        };

        return window;
    }
}