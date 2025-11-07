using MauiQRApp.Services;

namespace MauiQRApp.Views;

public partial class RegistrarClavePage : ContentPage
{
    private readonly DatabaseService _dbService;

    public RegistrarClavePage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        string clave = txtClave.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(clave))
        {
            await DisplayAlert("Error", "Debe ingresar una clave", "OK");
            return;
        }

        await _dbService.InsertarClaveAsync(clave);
        await DisplayAlert("Éxito", "Clave registrada correctamente", "OK");
        await Navigation.PopAsync();
    }
}
