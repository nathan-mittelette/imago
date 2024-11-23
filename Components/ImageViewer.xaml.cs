using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imago.Services;

namespace imago.Components;

public partial class ImageViewer : ContentView
{
    private ImageStateService? _imageStateService;

    public ImageViewer()
    {
        InitializeComponent();
    }

    public void ProvideImageStateService(ImageStateService imageStateService)
    {
        _imageStateService = imageStateService;
        _imageStateService.OnImageUpdated += OnImageUpdated;
    }

    private void OnImageUpdated()
    {
        ImageView.Source = _imageStateService?.ImportedImage;
    }
}