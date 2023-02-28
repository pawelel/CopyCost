using System;
using System.Windows;
using CopyCost.WPF.Categories;
using CopyCost.WPF.Customers;
using CopyCost.WPF.Main;
using CopyCost.WPF.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace CopyCost.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<PaymentsView>();
        services.AddSingleton<CategoriesView>();
        services.AddSingleton<CustomersView>();
        services.AddSingleton<CategoriesViewModel>();
        services.AddSingleton<CustomersViewModel>();
        _serviceProvider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainWindow.DataContext = mainViewModel;
        mainWindow.Show();
    }
}