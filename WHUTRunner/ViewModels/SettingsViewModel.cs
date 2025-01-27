using System.Collections.Generic;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using WHUTRunner.Models;
using WHUTRunner.Services;

namespace WHUTRunner.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly IPersistentService _persistentService;
        private AppSettings _settings = new();

        public SettingsViewModel(IPersistentService configService)
        {
            _persistentService = configService;
            LoadSettingsAsync().ConfigureAwait(false);
        }

        private async Task LoadSettingsAsync()
        {
            try
            {
                _settings = await _persistentService.LoadAppSettingsAsync();
            }
            catch (Exception ex)
            {
                // TODO: 处理错误
                Console.WriteLine($"加载设置失败: {ex.Message}");
            }
        }

        private async Task SaveSettingsAsync()
        {
            try
            {
                await _persistentService.SaveAppSettingsAsync(_settings);
            }
            catch (Exception ex)
            {
                // TODO: 处理错误
                Console.WriteLine($"保存设置失败: {ex.Message}");
            }
        }

        public bool AutoStart
        {
            get => _settings.AutoStart;
            set
            {
                _settings.AutoStart = value;
                this.RaisePropertyChanged(nameof(AutoStart));
                SaveSettingsAsync().ConfigureAwait(false);
            }
        }

        public bool MinimizeToTray
        {
            get => _settings.MinimizeToTray;
            set
            {
                _settings.MinimizeToTray = value;
                this.RaisePropertyChanged(nameof(MinimizeToTray));
                SaveSettingsAsync().ConfigureAwait(false);
            }
        }

        public Dictionary<string, string> DefaultParameters
        {
            get => _settings.DefaultParameters;
            set
            {
                _settings.DefaultParameters = value;
                this.RaisePropertyChanged(nameof(DefaultParameters));
                SaveSettingsAsync().ConfigureAwait(false);
            }
        }

        public string Title => "设置";
    }
}