using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using ViewModel;
using View.Views;

namespace View;

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
            var messenger = new WindowMessenger();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(messenger)
            };
            messenger.MainWindow = desktop.MainWindow;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = null
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
