using imago.Utils;
using imago.ViewModels;

namespace imago.Components;

public partial class ConfigurationPanelInformation : ContentView
{
    public ConfigurationPanelInformation()
    {
        InitializeComponent();
        BindingContext = ServiceLocator.Resolve<ImageViewModel>();
    }
}