using Avalonia;
using Optris.Icons.Avalonia;
using Optris.Icons.Avalonia.FontAwesome;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using microAPI.ViewModels;
using microAPI.Interfaces;
using microAPI.Repositories;


namespace microAPI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            IconProvider.Current
                .Register<FontAwesomeIconProvider>();
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();
                    
                    // Interfaces & Repositories
                    services.AddSingleton<ICollectionRepository, CollectionRepository>();
                    
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                })
                .Build();

            await AppHost.StartAsync();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            }
            base.OnFrameworkInitializationCompleted();
        }

    }

}
