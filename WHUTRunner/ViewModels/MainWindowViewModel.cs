using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Material.Icons;
using ReactiveUI;
using WHUTRunner.Models;
using WHUTRunner.Services;

namespace WHUTRunner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Window? _window;
        private NavigationItem? _selectedItem;
        private bool _isNavigationPaneOpen = true;
        private ViewModelBase? _currentPage;

        public MainWindowViewModel()
        {
            // 初始化导航项
            NavigationItems = new ObservableCollection<NavigationItem>
            {
                new() { Title = "功能", Icon = MaterialIconKind.Apps },
                new() { Title = "下载", Icon = MaterialIconKind.Download },
                new() { Title = "配置", Icon = MaterialIconKind.Environment },
                new() { Title = "设置", Icon = MaterialIconKind.Settings }
            };

            // 初始化命令
            ToggleNavigationPaneCommand = ReactiveCommand.Create(ToggleNavigationPane);
            MinimizeWindowCommand = ReactiveCommand.Create(MinimizeWindow);
            MaximizeWindowCommand = ReactiveCommand.Create(MaximizeWindow);
            CloseWindowCommand = ReactiveCommand.Create(CloseWindow);

            // 设置默认选中项
            SelectedItem = NavigationItems[0];

            // 监听导航项变化
            this.WhenAnyValue(x => x.SelectedItem)
                .Subscribe(OnNavigationItemChanged);
        }

        private void OnNavigationItemChanged(NavigationItem? item)
        {
            if (item == null) return;

            CurrentPage = item.Title switch
            {
                "功能" => new FeaturesViewModel(new ScriptRunService()),
                "下载" => new DownloadsViewModel(new ScriptPackageService()),
                "配置" => new ConfigViewModel(
                    new EnvironmentService(),
                    new PersistentService(),
                    _window?.StorageProvider),
                "设置" => new SettingsViewModel(new PersistentService()),
                _ => CurrentPage
            };
        }

        public void Initialize(Window window)
        {
            _window = window;
        }

        public ObservableCollection<NavigationItem> NavigationItems { get; }

        public NavigationItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != null)
                    _selectedItem.IsSelected = false;

                this.RaiseAndSetIfChanged(ref _selectedItem, value);

                if (_selectedItem != null)
                    _selectedItem.IsSelected = true;
            }
        }

        public ViewModelBase? CurrentPage
        {
            get => _currentPage;
            private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public bool IsNavigationPaneOpen
        {
            get => _isNavigationPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isNavigationPaneOpen, value);
        }

        public void ToggleNavigationPane()
        {
            IsNavigationPaneOpen = !IsNavigationPaneOpen;
        }

        public void MinimizeWindow()
        {
            if (_window != null)
            {
                _window.WindowState = WindowState.Minimized;
            }
        }

        public void MaximizeWindow()
        {
            if (_window != null)
            {
                _window.WindowState = _window.WindowState == WindowState.FullScreen ?
                    WindowState.Normal : WindowState.FullScreen;
            }
        }

        public void CloseWindow()
        {
            _window?.Close();
        }

        public ICommand ToggleNavigationPaneCommand { get; }
        public ICommand MinimizeWindowCommand { get; }
        public ICommand MaximizeWindowCommand { get; }
        public ICommand CloseWindowCommand { get; }
    }
}
