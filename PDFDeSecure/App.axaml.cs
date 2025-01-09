using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace PDFDeSecure;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new PDFDeSecureAvalonia();

            desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnLastWindowClose;
        }

        base.OnFrameworkInitializationCompleted();
    }
}