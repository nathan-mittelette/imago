using System.Collections.ObjectModel;
using System.ComponentModel;
using SkiaSharp;

namespace imago.ViewModels;

public class ResizeViewModels : INotifyPropertyChanged
{
    private int _originalWidth;
    private int _originalHeight;

    private int _width;
    private int _height;
    private int _quality;
    private string? _format;
    private ObservableCollection<string>? _availableFormats;

    public ObservableCollection<string>? AvailableFormats
    {
        get => _availableFormats;
        set
        {
            _availableFormats = value;
            OnPropertyChanged(nameof(AvailableFormats));
        }
    }

    public string? Format
    {
        get => _format;
        set
        {
            _format = value;
            OnPropertyChanged(nameof(Format));
        }
    }

    public int Width
    {
        get => _width;
        set
        {
            _width = value;
            OnPropertyChanged(nameof(Width));
            UpdateHeight();
        }
    }

    public int Height
    {
        get => _height;
        set
        {
            _height = value;
            OnPropertyChanged(nameof(Height));
            UpdateWidth();
        }
    }

    public int Quality
    {
        get => _quality;
        set
        {
            _quality = value;
            OnPropertyChanged(nameof(Quality));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void InitValues(SKBitmap skBitmap, string format)
    {
        Width = skBitmap.Width;
        Height = skBitmap.Height;
        Quality = 100;
        Format = format.ToUpper();
        _originalHeight = skBitmap.Height;
        _originalWidth = skBitmap.Width;

        var availableFormats = new [] { "JPEG", "WEBP", "PNG" };
        AvailableFormats = new ObservableCollection<string>(availableFormats);
    }

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void UpdateHeight()
    {
        if (_originalWidth > 0)
        {
            _height = (int)((double)_originalHeight * _width / _originalWidth);
            OnPropertyChanged(nameof(Height));
        }
    }

    private void UpdateWidth()
    {
        if (_originalHeight > 0)
        {
            _width = (int)((double)_originalWidth * _height / _originalHeight);
            OnPropertyChanged(nameof(Width));
        }
    }
}