using imago.Services;
using imago.Utils;
using imago.ViewModels;
using SkiaSharp;

namespace imago.Components;

public partial class ConfigurationPanelConverter : ContentView
{
    private readonly ImageConverterService _imageConverterService;

    public ConfigurationPanelConverter()
    {
        InitializeComponent();
        BindingContext = ServiceLocator.Resolve<ResizeViewModels>();
        _imageConverterService = ServiceLocator.Resolve<ImageConverterService>();
    }

    private async void OnConvertButtonClicked(object sender, EventArgs e)
    {
        ResizeViewModels resizeViewModels = (ResizeViewModels)BindingContext;

        var skFormat = Enum.Parse<SKEncodedImageFormat>(resizeViewModels.Format!, true);

        await _imageConverterService.ConvertImage(skFormat, resizeViewModels.Width,
            resizeViewModels.Height,
            resizeViewModels.Quality);
    }
}