using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using WHUTRunner.Models;
using WHUTRunner.Services;

namespace WHUTRunner.ViewModels
{
    public class DownloadsViewModel : ViewModelBase
    {
        private string _searchText = "";
        private bool _isLoading;
        private ObservableCollection<ScriptPackage> _filteredPackages;
        private ObservableCollection<ScriptPackage> _allPackages;
        private readonly IScriptPackageService _packageService;

        public DownloadsViewModel(IScriptPackageService packageService)
        {
            _packageService = packageService;
            _allPackages = new ObservableCollection<ScriptPackage>();
            _filteredPackages = new ObservableCollection<ScriptPackage>();

            // 初始化命令
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadPackagesAsync);
            InstallCommand = ReactiveCommand.CreateFromTask<ScriptPackage>(InstallPackageAsync);
            UninstallCommand = ReactiveCommand.CreateFromTask<ScriptPackage>(UninstallPackageAsync);

            // 加载包列表
            LoadPackagesAsync().ConfigureAwait(false);

            // 监听搜索文本变化
            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => FilterPackages());
        }

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public ObservableCollection<ScriptPackage> FilteredPackages
        {
            get => _filteredPackages;
            private set => this.RaiseAndSetIfChanged(ref _filteredPackages, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand InstallCommand { get; }
        public ICommand UninstallCommand { get; }

        private async Task LoadPackagesAsync()
        {
            IsLoading = true;

            try
            {
                var packages = await _packageService.GetAvailablePackagesAsync();
                var installedPackages = await _packageService.GetInstalledPackagesAsync();
                
                // 更新已安装状态
                foreach (var package in packages)
                {
                    package.IsInstalled = installedPackages.Any(p => p.Id == package.Id);
                }
                
                _allPackages = new ObservableCollection<ScriptPackage>(packages);
                FilterPackages();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterPackages()
        {
            var query = _allPackages.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchText = SearchText.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchText) ||
                    p.Description.ToLower().Contains(searchText) ||
                    p.Author.ToLower().Contains(searchText));
            }

            FilteredPackages = new ObservableCollection<ScriptPackage>(query);
        }

        private async Task InstallPackageAsync(ScriptPackage package)
        {
            if (package.IsInstalling) return;

            package.IsInstalling = true;
            try
            {
                await _packageService.InstallPackageAsync(package);
            }
            finally
            {
                package.IsInstalling = false;
            }
        }

        private async Task UninstallPackageAsync(ScriptPackage package)
        {
            if (package.IsInstalling) return;

            package.IsInstalling = true;
            try
            {
                await _packageService.UninstallPackageAsync(package);
            }
            finally
            {
                package.IsInstalling = false;
            }
        }

        public string Title => "下载";
    }
} 