using imago.Components;
using imago.Services;

namespace imago;

public partial class MainPage : ContentPage
{
    private readonly ImageStateService _imageStateService;

    public MainPage(ImageStateService imageStateService)
    {
        InitializeComponent();

        _imageStateService = imageStateService;
        _imageStateService.OnImageUpdated += OnImageUpdated;
        
        ImageSelector.ImageStateService = _imageStateService;
        ConfigurationPanel.ProvideImageStateService(_imageStateService);
        ImageViewer.ProvideImageStateService(_imageStateService);
    }

    private void OnImageUpdated()
    {
        if (_imageStateService.ImportedImage != null)
        {
            ImageSelector.IsVisible = false;
            ConfigurationPanelGrid.IsVisible = true;
        }
        else
        {
            ImageSelector.IsVisible = true;
            ConfigurationPanelGrid.IsVisible = false;
        }
    }
}