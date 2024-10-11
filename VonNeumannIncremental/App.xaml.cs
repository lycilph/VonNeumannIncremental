using NLog;
using System.Windows;
using VonNeumannIncremental.Core;

namespace VonNeumannIncremental;

public partial class App : Application
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        logger.Debug("Starting game");

        var game = new Game();

        var vm = new MainWindowViewModel(game);
        vm.Reset();
        vm.Start();

        var win = new MainWindow
        {
            DataContext = vm
        };
        win.Show();
    }
}
