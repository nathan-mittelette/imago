using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imago.Services;

namespace imago.Components;

public partial class ImageSelector : ContentView
{
    public ImageStateService? ImageStateService { get; set; }

    public ImageSelector()
    {
        InitializeComponent();

        PlaceholderView.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () => { await ImportImage(); })
        });
    }

    private async Task ImportImage()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Sélectionnez une image",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            await using (var stream = await result.OpenReadAsync())
            {
                ImageStateService!.UpdateImage(ImageSource.FromFile(result.FullPath), stream, result.ContentType,
                    result.FileName);
            }
        }
    }
}