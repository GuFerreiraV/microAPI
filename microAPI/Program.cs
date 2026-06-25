using Avalonia;
using System;

namespace microAPI;

class Program
{
    // ponto de entrada para aplicação
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // config do construtor de aplicação do avalonia
    public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .LogToTrace();
}