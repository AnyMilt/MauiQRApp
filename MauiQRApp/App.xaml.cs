using MauiQRApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MauiQRApp;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    public App(IServiceProvider serviceProvider, MainPage loginPage)
    {
        InitializeComponent();
        Services = serviceProvider;
        MainPage = new NavigationPage(loginPage);
    }
}
