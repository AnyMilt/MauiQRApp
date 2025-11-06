using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiQRApp.Services;
using System.Text;
using System.Text.Json;
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
        try
        {
            // Usa el service provider global definido en App.cs
            var scanQRPage = App.Services.GetService<ScanQRPage>();

            if (scanQRPage != null)
            {
                await Navigation.PushAsync(scanQRPage);
            }
            else
            {
                await DisplayAlert("Error", "No se pudo abrir la cámara de escaneo.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un problema al abrir el escáner: {ex.Message}", "OK");
        }
    }

    private async void OnAsistenciaVirtualClicked(object sender, EventArgs e)
    {
        try
        {
            string idDocente = Preferences.Get("IdDocente", string.Empty);

            if (string.IsNullOrEmpty(idDocente))
            {
                await MostrarMensajeTemporal("⚠️ Debe registrar un docente primero.");
                return;
            }

            var datos = new
            {
                idDocente,
                idDispositivo = DeviceInfo.Current.Model,
                lat = "0", // No es necesario GPS para clases virtuales
                lng = "0",
                tipo = "Entrada",
                modo = "virtual",
                fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            // Envía el registro directamente al backend (sin QR)
            var resultado = await EnviarAsistenciaVirtual(datos);

            if (resultado)
                await MostrarMensajeTemporal("✅ Asistencia virtual enviada correctamente.");
            else
                await MostrarMensajeTemporal("⚠️ No se pudo enviar la asistencia virtual.");
        }
        catch (Exception ex)
        {
            await MostrarMensajeTemporal($"❌ Error: {ex.Message}");
        }
    }
    public async Task<bool> EnviarAsistenciaVirtual(object datos)
    {
        try
        {
            using var client = new HttpClient();
            string baseUrl = "https://tu-servidor.ngrok-free.app/registrar"; // <-- cambia según tu dominio local o ngrok
            var json = JsonSerializer.Serialize(datos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

}

