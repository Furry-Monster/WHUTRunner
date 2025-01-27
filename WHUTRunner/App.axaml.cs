using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WHUTRunner.ViewModels;
using WHUTRunner.Views;

namespace WHUTRunner
{
    public partial class App : Application
    {
        // 单例
        private static App? _instance;
        public static App Instance => _instance ??= new App();

        // 初始化应用程序
        public override void Initialize()
        {
            _instance = this;
            AvaloniaXamlLoader.Load(this);
        }

        // 当框架初始化完成时调用
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // 这里我们提前将主窗口的vm注入到window的DataContext中
                // 避免出现vm引用不一致的情况，导致_windows引用出错，标题栏无效等问题
                var mainWindow = new MainWindow();
                var viewModel = new MainWindowViewModel();
                viewModel.Initialize(mainWindow);
                mainWindow.DataContext = viewModel;
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}