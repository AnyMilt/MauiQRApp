using Microsoft.Extensions.DependencyInjection;
using MauiQRApp.Services;

namespace MauiQRApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _dbService;

    public LoginPage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
        _ = _dbService.SeedDataAsync();
    }

    private async void OnScanClicked(object sender, EventArgs e)
    {
        // Simulación del escaneo QR — reemplazar con ZXing.Net.Maui lector real si deseas
        txtIdDocente.Text = await DisplayPromptAsync("Simulación", "Ingrese ID del docente escaneado:");
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtIdDocente.Text) || string.IsNullOrEmpty(txtClave.Text))
        {
            await DisplayAlert("Error", "Debe ingresar ID y clave", "OK");
            return;
        }

        bool valido = await _dbService.ValidarClaveAsync(txtClave.Text);
        if (valido)
        {
            Preferences.Set("IdDocente", txtIdDocente.Text);
            await DisplayAlert("Éxito", "Docente registrado correctamente", "OK");
            var main = App.Services.GetService<MainPage>();
            if (main != null) 
                await Navigation.PushAsync(main); 
            else 
                await Navigation.PushAsync(new MainPage(new GeoService(), new QRService()));
        }
        else
        {
            await DisplayAlert("Error", "Clave incorrecta", "OK");
        }
    }

    private async void OnRegistrarClaveClicked(object sender, EventArgs e)
    {
        var registrarClavePage = App.Services.GetService<RegistrarClavePage>(); // ✅ correcta
        await Navigation.PushAsync(registrarClavePage);
    }
}
