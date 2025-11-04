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
        cameraView.IsDetecting = false;

        var code = e.Results.FirstOrDefault()?.Value;

        if (!string.IsNullOrEmpty(code))
        {
            try
            {
                // Intentamos interpretar el QR como URL
                var uri = new Uri(code);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

                // Asumimos que el parámetro del docente es "docente" o "id"
                var idDocente = query["docente"] ?? query["id"];

                if (string.IsNullOrEmpty(idDocente))
                {
                    await DisplayAlert("Error", "El QR no contiene un ID de docente válido.", "OK");
                    cameraView.IsDetecting = true;
                    return;
                }

                // Guardamos solo el ID numérico
                Preferences.Set("IdDocente", idDocente);
                await _db.SaveDocenteIdAsync(idDocente);

                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("✅ Éxito", $"Docente registrado: {idDocente}", "OK");
                    await Navigation.PopAsync();
                });
            }
            catch
            {
                await DisplayAlert("Error", "El código QR escaneado no es válido.", "OK");
                cameraView.IsDetecting = true;
            }
        }
        else
        {
            await DisplayAlert("Error", "No se pudo leer el código. Intente nuevamente.", "OK");
            cameraView.IsDetecting = true;
        }
    }


    private void OnToggleTorchClicked(object sender, EventArgs e)
    {
        _isTorchOn = !_isTorchOn;
        
        cameraView.IsTorchOn = _isTorchOn;

    }
}
