using imago.Models;
using imago.ViewModels;
using SkiaSharp;

namespace imago.Services;

public class ImageLoaderService
{
    private readonly ImageViewModel _imageViewModel;
    private readonly ResizeViewModels _resizeViewModels;
    private readonly ToastService _toastService;
    private readonly GlobalStorageService _globalStorageService;

    public ImageLoaderService(ImageViewModel imageViewModel, ToastService toastService,
        GlobalStorageService globalStorageService, ResizeViewModels resizeViewModels)
    {
        _imageViewModel = imageViewModel;
        _toastService = toastService;
        _globalStorageService = globalStorageService;
        _resizeViewModels = resizeViewModels;
    }

    public async Task LoadImage(string? fullPath)
    {
        if (fullPath == null)
        {
            await _toastService.ShowToast("Aucune image sélectionnée");
            return;
        }

        var image = ImageSource.FromFile(fullPath);
        
        await using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            var skBitmap = SKBitmap.Decode(stream);

            if (skBitmap == null)
            {
                await _toastService.ShowToast("Impossible de charger l'image");
                return;
            }

            var filename = Path.GetFileNameWithoutExtension(fullPath);
            var extension = Path.GetExtension(fullPath).TrimStart('.');
            var fileSize = new FileInfo(fullPath).Length;

            var imageData = new ImageData(extension, filename, skBitmap.Width,
                skBitmap.Height, GetSize(fileSize));

            _imageViewModel.InitValues(image, imageData);
            _resizeViewModels.InitValues(skBitmap, extension);

            _globalStorageService.CurrentImageSkBitmap = skBitmap;
        }
    }

    private string GetSize(long byteCount)
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