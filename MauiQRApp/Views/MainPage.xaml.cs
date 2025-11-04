using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiQRApp.Services;
using ZXing.Net.Maui;

namespace MauiQRApp.Views;

public partial class MainPage : ContentPage
{
    private readonly GeoService _geo;
    private readonly QRService _qr;

    public MainPage(GeoService geo, QRService qr)
    {
        InitializeComponent();
        _geo = geo;
        _qr = qr;
    }

    private async void OnEntradaClicked(object sender, EventArgs e)
    {
        await GenerarQR("Entrada");
    }

    private async void OnSalidaClicked(object sender, EventArgs e)
    {
        await GenerarQR("Salida");
    }

    private async Task GenerarQR(string tipo)
    {
        string idDocente = Preferences.Get("IdDocente", string.Empty);
        var location = await _geo.GetLocationAsync();

        if (location == null)
        {
            await MostrarMensajeTemporal("⚠️ No se pudo obtener ubicación GPS. Se continuará sin coordenadas.");
        }


        var datos = new
        {
            idDocente,
            idDispositivo = DeviceInfo.Current.Model,
            lat = location?.Latitude.ToString() ?? "0",
            lng = location?.Longitude.ToString() ?? "0",
            tipo,
            fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var qrValue = _qr.GenerarQR(datos);  // Devuelve string, no ImageSource
        await Navigation.PushAsync(new QRResultPage(qrValue));
    }
    private async Task MostrarMensajeTemporal(string mensaje)
    {
        var toast = Toast.Make(mensaje, ToastDuration.Short, 14);
        await toast.Show();
    }

    private async void OnRegistrarDocenteClicked(object sender, EventArgs e)
    {
        var scanQRPage = App.Current.Handler.MauiContext.Services.GetService<ScanQRPage>();
        if (scanQRPage != null)
            await Navigation.PushAsync(scanQRPage);
        else
            await DisplayAlert("Error", "No se pudo abrir la cámara de escaneo.", "OK");
    }
}

