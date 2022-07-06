using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Employees.Services;
using Employees.ViewModels;
using Employees.Views;

namespace Employees;

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

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(new Database()),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}