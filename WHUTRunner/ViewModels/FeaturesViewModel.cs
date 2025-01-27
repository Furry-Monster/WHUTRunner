using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using WHUTRunner.Models;
using WHUTRunner.Services;

namespace WHUTRunner.ViewModels
{
    public class FeaturesViewModel : ViewModelBase
    {
        private readonly IScriptRunService _scriptRunService;
        private ScriptItem? _selectedScript;
        private bool _isRunning;

        public FeaturesViewModel(IScriptRunService scriptRunService)
        {
            _scriptRunService = scriptRunService;

            // 加载可用的脚本配置
            Scripts = new ObservableCollection<ScriptItem>
            {
                new ScriptItem
                {
                    Name = "理工学堂一键满分",
                    Description = "自动完成所有课程作业并获得满分",
                    Type = "Python",
                    Parameters = new()
                    {
                        { "username", "" },
                        { "password", "" }
                    }
                },
                new ScriptItem
                {
                    Name = "一键看完视频",
                    Description = "自动完成所有视频观看",
                    Type = "Python",
                    Parameters = new()
                    {
                        { "courseId", "" }
                    }
                }
            };

            // 初始化命令
            RunCommand = ReactiveCommand.CreateFromTask(RunScriptAsync);
            StopCommand = ReactiveCommand.CreateFromTask(StopScriptAsync);
        }

        public ObservableCollection<ScriptItem> Scripts { get; }

        public ScriptItem? SelectedScript
        {
            get => _selectedScript;
            set => this.RaiseAndSetIfChanged(ref _selectedScript, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => this.RaiseAndSetIfChanged(ref _isRunning, value);
        }

        public ICommand RunCommand { get; }
        public ICommand StopCommand { get; }

        private async Task RunScriptAsync()
        {
            if (SelectedScript == null || IsRunning) return;

            IsRunning = true;
            try
            {
                await _scriptRunService.RunAsync(SelectedScript);
            }
            finally
            {
                IsRunning = false;
            }
        }

        private async Task StopScriptAsync()
        {
            if (!IsRunning) return;

            await _scriptRunService.StopAsync();
            IsRunning = false;
        }


        public string Title => "功能";

    }
}
