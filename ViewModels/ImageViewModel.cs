using System.ComponentModel;
using imago.Models;

namespace imago.ViewModels;

public class ImageViewModel : INotifyPropertyChanged
{
    private ImageSource? _image = null;
    private ImageData? _imageData = null;

    public ImageSource? Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(IsImageVisible));
        }
    }

    public ImageData? ImageData
    {
        get => _imageData;
        set
        {
            _imageData = value;
            OnPropertyChanged(nameof(ImageData));
        }
    }

    public bool IsImageVisible => Image != null;

    public void InitValues(ImageSource image, ImageData imageData)
    {
        Image = image;
        ImageData = imageData;
    }

    public void ResetValues()
    {
        Image = null;
        ImageData = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}