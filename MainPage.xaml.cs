using imago.Utils;
using imago.ViewModels;

namespace imago;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = ServiceLocator.Resolve<ImageViewModel>();
    }
}