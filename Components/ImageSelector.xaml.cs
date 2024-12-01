using imago.Services;
using imago.Utils;

namespace imago.Components;

public partial class ImageSelector : ContentView
{
    private readonly ImageLoaderService _imageLoaderService;

    public ImageSelector()
    {
        InitializeComponent();

        _imageLoaderService = ServiceLocator.Resolve<ImageLoaderService>();

        PlaceholderView.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async void () => { await ImportImage(); })
        });
    }

    private async Task ImportImage()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Sélectionnez une image",
            FileTypes = FilePickerFileType.Images
        });

        await _imageLoaderService.LoadImage(result?.FullPath);
    }
}