using imago.Utils;
using imago.ViewModels;

namespace imago.Components;

public partial class ConfigurationPanel : ContentView
{
    private readonly ImageViewModel _imageViewModel;

    public ConfigurationPanel()
    {
        InitializeComponent();
        _imageViewModel = ServiceLocator.Resolve<ImageViewModel>();
    }

    private void OnCleanButtonClick(object sender, EventArgs e)
    {
        _imageViewModel.ResetValues();
    }
}