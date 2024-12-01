using CommunityToolkit.Maui.Storage;
using imago.ViewModels;
using SkiaSharp;

namespace imago.Services;

public class ImageConverterService
{
    private readonly ImageViewModel _imageViewModel;
    private readonly ResizeViewModels _resizeViewModels;
    private readonly ToastService _toastService;
    private readonly GlobalStorageService _globalStorageService;

    public ImageConverterService(ImageViewModel imageViewModel, ResizeViewModels resizeViewModels, ToastService toastService, GlobalStorageService globalStorageService)
    {
        _imageViewModel = imageViewModel;
        _resizeViewModels = resizeViewModels;
        _toastService = toastService;
        _globalStorageService = globalStorageService;
    }

    public async Task ConvertImage(SKEncodedImageFormat format, int width, int height, int quality)
    {
        if (_globalStorageService.CurrentImageSkBitmap == null)
        {
            await _toastService.ShowToast("Aucune image à convertir");
            return;
        }

        var bitmap = _globalStorageService.CurrentImageSkBitmap;

        bitmap = bitmap.Resize(new SKImageInfo(width: _resizeViewModels.Width, height: _resizeViewModels.Height), SKFilterQuality.High);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(format, quality);

        if (data == null)
        {
            await _toastService.ShowToast("Ce format n'est pas pris en compte par le sytème");
            return;
        }

        var stream = data.AsStream();

        var fileName = Path.GetFileNameWithoutExtension(_imageViewModel.ImageData!.Name) + "." + format.ToString().ToLower();
        // Save the stream to a file
        await FileSaver.Default.SaveAsync(fileName, stream);
    }
}