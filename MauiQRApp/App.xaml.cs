using MauiQRApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MauiQRApp;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;
    private readonly Page _mainPage;

    public App(IServiceProvider serviceProvider, MainPage loginPage)
    {
        InitializeComponent();
        Services = serviceProvider;
        _mainPage = new NavigationPage(loginPage);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_mainPage);
    }
}
