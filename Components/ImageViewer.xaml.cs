using imago.Utils;
using imago.ViewModels;

namespace imago.Components;

public partial class ImageViewer : ContentView
{
    public ImageViewer()
    {
        InitializeComponent();
        BindingContext = ServiceLocator.Resolve<ImageViewModel>();
    }
}