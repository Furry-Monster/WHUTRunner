using System;
using ReactiveUI;

namespace WHUTRunner.Models
{
    public class ScriptPackage : ReactiveObject
    {
        private string _id = "";
        private string _name = "";
        private string _description = "";
        private string _author = "";
        private string _version = "";
        private string _type = "Python";
        private string _iconUrl = "";
        private bool _isInstalled;
        private bool _isInstalling;
        private long _downloadCount;
        private DateTime _lastUpdated;

        public string Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public string Author
        {
            get => _author;
            set => this.RaiseAndSetIfChanged(ref _author, value);
        }

        public string Version
        {
            get => _version;
            set => this.RaiseAndSetIfChanged(ref _version, value);
        }

        public string Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        public string IconUrl
        {
            get => _iconUrl;
            set => this.RaiseAndSetIfChanged(ref _iconUrl, value);
        }

        public bool IsInstalled
        {
            get => _isInstalled;
            set => this.RaiseAndSetIfChanged(ref _isInstalled, value);
        }

        public bool IsInstalling
        {
            get => _isInstalling;
            set => this.RaiseAndSetIfChanged(ref _isInstalling, value);
        }

        public long DownloadCount
        {
            get => _downloadCount;
            set => this.RaiseAndSetIfChanged(ref _downloadCount, value);
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set => this.RaiseAndSetIfChanged(ref _lastUpdated, value);
        }
    }
} 