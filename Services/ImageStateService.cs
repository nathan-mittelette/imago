using CommunityToolkit.Maui.Storage;
using SkiaSharp; // Ajoutez le package SkiaSharp via NuGet

namespace imago.Services;

public class ImageStateService
{
    // Propriété pour stocker l'image sélectionnée
    public ImageSource? ImportedImage { get; private set; }

    // Propriété pour stocker le format de l'image
    public string? ImageFormat { get; private set; }

    public int? ImageWidth { get; private set; }

    public int? ImageHeight { get; private set; }

    public string? ImageSize { get; private set; }

    public string? ImageName { get; private set; }

    private SKBitmap? _imageInfo;

    // Événement déclenché lorsqu'une image est mise à jour
    public event Action? OnImageUpdated;

    // Méthode pour mettre à jour l'image
    public void UpdateImage(ImageSource image, Stream fileStream, string format, string name)
    {
        ImportedImage = image;
        ImageFormat = format;
        ImageName = name;

        _imageInfo = SKBitmap.Decode(fileStream);

        if (_imageInfo != null)
        {
            ImageHeight = _imageInfo.Height;
            ImageWidth = _imageInfo.Width;
            ImageSize = GetSize(_imageInfo.ByteCount);
        }

        // Notifier les abonnés que l'image a changé
        OnImageUpdated?.Invoke();
    }

    // Méthode pour réinitialiser l'état
    public void ClearImage()
    {
        ImportedImage = null;
        ImageFormat = null;

        // Notifier les abonnés
        OnImageUpdated?.Invoke();
    }

    public async Task ConvertTo(SKEncodedImageFormat format, int quality = 100, int width = 0, int height = 0)
    {
        if (_imageInfo == null)
        {
            return;
        }

        SKBitmap bitmap = _imageInfo;

        if (width != 0 && height != 0)
        {
            bitmap = _imageInfo.Resize(new SKImageInfo(width: width, height: height), SKFilterQuality.High);
        }

        using var image = SKImage.FromBitmap(bitmap);

        using var data = image.Encode(format, quality);

        var stream = data.AsStream();

        var fileName = Path.GetFileNameWithoutExtension(ImageName) + "." + format.ToString().ToLower();
        // Save the stream to a file
        await FileSaver.Default.SaveAsync(fileName, stream);
    }

    private string GetSize(int byteCount)
    {
        if (byteCount < 1024)
        {
            return byteCount + " B";
        }
        else if (byteCount < 1024 * 1024)
        {
            return byteCount / 1024 + " KB";
        }
        else
        {
            return byteCount / (1024 * 1024) + " MB";
        }
    }
}