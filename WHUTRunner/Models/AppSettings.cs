using System.Collections.Generic;
using ReactiveUI;

namespace WHUTRunner.Models
{
    public class AppSettings : ReactiveObject
    {
        private bool _autoStart;
        private bool _minimizeToTray;
        private Dictionary<string, string> _defaultParameters = new()
        {
            {"username",""},
            {"password",""},
        };

        public bool AutoStart
        {
            get => _autoStart;
            set => this.RaiseAndSetIfChanged(ref _autoStart, value);
        }

        public bool MinimizeToTray
        {
            get => _minimizeToTray;
            set => this.RaiseAndSetIfChanged(ref _minimizeToTray, value);
        }

        public Dictionary<string, string> DefaultParameters
        {
            get => _defaultParameters;
            set => this.RaiseAndSetIfChanged(ref _defaultParameters, value);
        }
    }
}