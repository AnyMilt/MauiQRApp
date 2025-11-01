using Microsoft.Maui.Hosting;
using MauiQRApp.Services;
using MauiQRApp.Views;
using ZXing.Net.Maui.Controls;
using CommunityToolkit.Maui;

namespace MauiQRApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseBarcodeReader() // ✅ ESTE es el correcto para ZXing.Net.MAUI
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ✅ Servicios
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<GeoService>();
        builder.Services.AddSingleton<QRService>();

        // ✅ Páginas registradas
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<QRResultPage>();
        builder.Services.AddTransient<RegistrarClavePage>();
        builder.Services.AddTransient<ScanQRPage>(); // ✅ importante

        return builder.Build();
    }
}
