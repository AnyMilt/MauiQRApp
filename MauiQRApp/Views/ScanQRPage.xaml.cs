using MauiQRApp.Services;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace MauiQRApp.Views;

public partial class ScanQRPage : ContentPage
{
    private readonly DatabaseService _db;
    private bool _isTorchOn = false;
    public ScanQRPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void CameraView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        // Evita múltiples lecturas simultáneas
        cameraView.IsDetecting = false;

        var code = e.Results.FirstOrDefault()?.Value;

        if (!string.IsNullOrEmpty(code))
        {
            // Guardar el ID escaneado
            Preferences.Set("IdDocente", code);
            await _db.SaveDocenteIdAsync(code);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Éxito", $"Docente registrado: {code}", "OK");
                await Navigation.PopAsync(); // volver a la pantalla principal
            });
        }
        else
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error", "No se pudo leer el código. Intente nuevamente.", "OK");
                cameraView.IsDetecting = true;
            });
        }
    }

    private void OnToggleTorchClicked(object sender, EventArgs e)
    {
        _isTorchOn = !_isTorchOn;
        
        cameraView.IsTorchOn = _isTorchOn;

    }
}
