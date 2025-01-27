using System.Collections.Generic;
using ReactiveUI;

namespace WHUTRunner.Models
{
    public class EnvironmentConfig : ReactiveObject
    {
        private string _pythonPath = "python";
        private string _pipPath = "pip";
        private string _nodePath = "node";
        private string _npmPath = "npm";
        private List<string> _pythonPackages = new();
        private List<string> _nodePackages = new();

        public string PythonPath
        {
            get => _pythonPath;
            set => this.RaiseAndSetIfChanged(ref _pythonPath, value);
        }

        public string PipPath
        {
            get => _pipPath;
            set => this.RaiseAndSetIfChanged(ref _pipPath, value);
        }

        public string NodePath
        {
            get => _nodePath;
            set => this.RaiseAndSetIfChanged(ref _nodePath, value);
        }

        public string NpmPath
        {
            get => _npmPath;
            set => this.RaiseAndSetIfChanged(ref _npmPath, value);
        }

        public List<string> PythonPackages
        {
            get => _pythonPackages;
            set => this.RaiseAndSetIfChanged(ref _pythonPackages, value);
        }

        public List<string> NodePackages
        {
            get => _nodePackages;
            set => this.RaiseAndSetIfChanged(ref _nodePackages, value);
        }
    }
} 