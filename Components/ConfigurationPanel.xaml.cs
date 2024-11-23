using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imago.Services;
using SkiaSharp;

namespace imago.Components;

public partial class ConfigurationPanel : ContentView
{
    private ImageStateService? _imageStateService;

    private int _originalWidth;
    private int _originalHeight;
    private bool _isUpdating; // Pour éviter des boucles infinies entre les deux champs

    public ConfigurationPanel()
    {
        InitializeComponent();

        FormatPicker.ItemsSource = Enum.GetValues(typeof(SKEncodedImageFormat)).Cast<SKEncodedImageFormat>()
            .Select(x => x.ToString().ToUpper()).ToList();
    }

    public void ProvideImageStateService(ImageStateService imageStateService)
    {
        _imageStateService = imageStateService;

        _imageStateService.OnImageUpdated += OnImageUpdated;
    }

    private void OnImageUpdated()
    {
        FormatLabel.Text = _imageStateService?.ImageFormat ?? "N/A";
        SizeLabel.Text = _imageStateService?.ImageSize ?? "N/A";
        if (_imageStateService?.ImageHeight != null)
        {
            HeightLabel.Text = _imageStateService.ImageHeight + " px";
        }
        else
        {
            HeightLabel.Text = "N/A";
        }

        if (_imageStateService?.ImageWidth != null)
        {
            WidthLabel.Text = _imageStateService.ImageWidth + " px";
        }
        else
        {
            WidthLabel.Text = "N/A";
        }

        NameLabel.Text = _imageStateService?.ImageName ?? "N/A";

        _originalWidth = _imageStateService?.ImageWidth ?? 0;
        _originalHeight = _imageStateService?.ImageHeight ?? 0;

        _isUpdating = true;
        WidthEntry.Text = _imageStateService?.ImageWidth?.ToString() ?? "N/A";
        HeightEntry.Text = _imageStateService?.ImageHeight?.ToString() ?? "N/A";
        _isUpdating = false;
        FormatPicker.SelectedItem = null;
        QualitySlider.Value = 100;
    }

    private void OnConvertButtonClicked(object sender, EventArgs e)
    {
        string selectedFormat = FormatPicker.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(selectedFormat))
        {
            Application.Current.MainPage.DisplayAlert("Erreur", "Veuillez sélectionner un format de sortie.", "OK");
            return;
        }

        SKEncodedImageFormat format =
            (SKEncodedImageFormat)Enum.Parse(typeof(SKEncodedImageFormat), selectedFormat, true);
        int quality = (int)QualitySlider.Value;

        // If Width or Height is updated, use the new values
        int width = string.IsNullOrEmpty(WidthEntry.Text) ? 0 : int.Parse(WidthEntry.Text);
        int height = string.IsNullOrEmpty(HeightEntry.Text) ? 0 : int.Parse(HeightEntry.Text);

        if (width != _originalWidth || height != _originalHeight)
        {
            _imageStateService?.ConvertTo(format, quality, width, height);
        }
        else
        {
            _imageStateService?.ConvertTo(format, quality);
        }
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        int value = (int)e.NewValue;
        QualityLabel.Text = value.ToString();
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        _imageStateService?.ClearImage();
    }

    private void OnWidthChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating || string.IsNullOrEmpty(WidthEntry.Text)) return;

        _isUpdating = true;

        if (double.TryParse(WidthEntry.Text, out double newWidth))
        {
            double scale = newWidth / _originalWidth; // Calculer le ratio
            double newHeight = Math.Round(_originalHeight * scale); // Calculer la nouvelle hauteur

            // Mettre à jour la hauteur liée
            HeightEntry.Text = newHeight.ToString();
        }

        _isUpdating = false;
    }

    private void OnHeightChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating || string.IsNullOrEmpty(HeightEntry.Text)) return;

        _isUpdating = true;

        if (double.TryParse(HeightEntry.Text, out double newHeight))
        {
            double scale = newHeight / _originalHeight; // Calculer le ratio
            double newWidth = Math.Round(_originalWidth * scale); // Calculer la nouvelle largeur

            // Mettre à jour la largeur liée
            WidthEntry.Text = newWidth.ToString();
        }

        _isUpdating = false;
    }
}