using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using WHUTRunner.Models;
using WHUTRunner.Services;
using Avalonia.Platform.Storage;
using System.Linq;
using DialogHostAvalonia;
using WHUTRunner.Views.Dialogs;

namespace WHUTRunner.ViewModels
{
    public class ConfigViewModel : ViewModelBase
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IPersistentService _persistentService;
        private readonly IStorageProvider? _storageProvider;
        private EnvironmentConfig _config = new();
        private bool _isChecking;
        private string _status = "";

        public ConfigViewModel(
            IEnvironmentService environmentService,
            IPersistentService configService,
            IStorageProvider? storageProvider = null)
        {
            _environmentService = environmentService;
            _persistentService = configService;
            _storageProvider = storageProvider;

            LoadConfigAsync().ConfigureAwait(false);

            CheckEnvironmentCommand = ReactiveCommand.CreateFromTask(CheckEnvironmentAsync);
            InstallDependenciesCommand = ReactiveCommand.CreateFromTask(InstallDependenciesAsync);
            BrowsePythonCommand = ReactiveCommand.CreateFromTask(BrowsePythonAsync);
            BrowsePipCommand = ReactiveCommand.CreateFromTask(BrowsePipAsync);
            BrowseNodeCommand = ReactiveCommand.CreateFromTask(BrowseNodeAsync);
            BrowseNpmCommand = ReactiveCommand.CreateFromTask(BrowseNpmAsync);
            AddPythonPackageCommand = ReactiveCommand.CreateFromTask(AddPythonPackageAsync);
            RemovePythonPackageCommand = ReactiveCommand.Create<string>(RemovePythonPackage);
            AddNodePackageCommand = ReactiveCommand.CreateFromTask(AddNodePackageAsync);
            RemoveNodePackageCommand = ReactiveCommand.Create<string>(RemoveNodePackage);
        }

        public EnvironmentConfig Config
        {
            get => _config;
            set => this.RaiseAndSetIfChanged(ref _config, value);
        }

        public bool IsChecking
        {
            get => _isChecking;
            set => this.RaiseAndSetIfChanged(ref _isChecking, value);
        }

        public string Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        public ICommand CheckEnvironmentCommand { get; }
        public ICommand InstallDependenciesCommand { get; }
        public ICommand BrowsePythonCommand { get; }
        public ICommand BrowsePipCommand { get; }
        public ICommand BrowseNodeCommand { get; }
        public ICommand BrowseNpmCommand { get; }
        public ICommand AddPythonPackageCommand { get; }
        public ICommand RemovePythonPackageCommand { get; }
        public ICommand AddNodePackageCommand { get; }
        public ICommand RemoveNodePackageCommand { get; }

        private async Task LoadConfigAsync()
        {
            try
            {
                Config = await _persistentService.LoadEnvironmentConfigAsync();
            }
            catch (Exception ex)
            {
                Status = $"加载配置失败: {ex.Message}";
            }
        }

        private async Task SaveConfigAsync()
        {
            try
            {
                await _persistentService.SaveEnvironmentConfigAsync(Config);
                Status = "配置已保存";
            }
            catch (Exception ex)
            {
                Status = $"保存配置失败: {ex.Message}";
            }
        }

        private async Task CheckEnvironmentAsync()
        {
            IsChecking = true;
            Status = "正在检查环境...";

            try
            {
                var result = await _environmentService.CheckEnvironmentAsync(Config);
                Status = result ? "环境检查通过" : "环境检查失败";
            }
            catch
            {
                Status = "环境检查出错";
            }
            finally
            {
                IsChecking = false;
            }
        }

        private async Task InstallDependenciesAsync()
        {
            IsChecking = true;
            Status = "正在安装依赖...";

            try
            {
                await _environmentService.InstallPythonPackagesAsync(Config.PythonPackages.ToArray());
                await _environmentService.InstallNodePackagesAsync(Config.NodePackages.ToArray());
                Status = "依赖安装完成";
            }
            catch
            {
                Status = "依赖安装失败";
            }
            finally
            {
                IsChecking = false;
            }
        }

        private async Task BrowsePythonAsync()
        {
            if (_storageProvider == null) return;
            var file = await BrowseExecutableFileAsync("选择 Python 解释器");
            if (file != null)
            {
                Config.PythonPath = file;
                await SaveConfigAsync();
            }
        }

        private async Task BrowsePipAsync()
        {
            if (_storageProvider == null) return;
            var file = await BrowseExecutableFileAsync("选择 pip 程序");
            if (file != null) Config.PipPath = file;
        }

        private async Task BrowseNodeAsync()
        {
            if (_storageProvider == null) return;
            var file = await BrowseExecutableFileAsync("选择 Node.js 解释器");
            if (file != null) Config.NodePath = file;
        }

        private async Task BrowseNpmAsync()
        {
            if (_storageProvider == null) return;
            var file = await BrowseExecutableFileAsync("选择 npm 程序");
            if (file != null) Config.NpmPath = file;
        }

        private async Task<string?> BrowseExecutableFileAsync(string title)
        {
            var options = new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("可执行文件")
                    {
                        Patterns = new[] { "*.exe" },
                        MimeTypes = new[] { "application/x-msdownload" }
                    },
                    new FilePickerFileType("所有文件")
                    {
                        Patterns = new[] { "*.*" },
                        MimeTypes = new[] { "application/octet-stream" }
                    }
                }
            };

            var files = await _storageProvider.OpenFilePickerAsync(options);
            return files.FirstOrDefault()?.Path.LocalPath;
        }

        private async Task AddPythonPackageAsync()
        {
            var dialog = new AddPackageDialog();
            var result = await DialogHost.Show(dialog, "RootDialog") as string;
            
            if (!string.IsNullOrWhiteSpace(result))
            {
                Config.PythonPackages.Add(result);
                await SaveConfigAsync();
            }
        }

        private void RemovePythonPackage(string package)
        {
            Config.PythonPackages.Remove(package);
            SaveConfigAsync().ConfigureAwait(false);
        }

        private async Task AddNodePackageAsync()
        {
            var dialog = new AddPackageDialog();
            var result = await DialogHost.Show(dialog, "RootDialog") as string;
            
            if (!string.IsNullOrWhiteSpace(result))
            {
                Config.NodePackages.Add(result);
                await SaveConfigAsync();
            }
        }

        private void RemoveNodePackage(string package)
        {
            Config.NodePackages.Remove(package);
            SaveConfigAsync().ConfigureAwait(false);
        }

        public string Title => "配置";
    }
} 