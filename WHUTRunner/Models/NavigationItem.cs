using Material.Icons;
using ReactiveUI;

namespace WHUTRunner.Models
{
    public class NavigationItem : ReactiveObject
    {
        private string _title = "";
        private MaterialIconKind _icon;
        private bool _isSelected;

        public NavigationItem()
        {
        }

        public NavigationItem(string title, MaterialIconKind icon)
        {
            _title = title;
            _icon = icon;
        }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public MaterialIconKind Icon
        {
            get => _icon;
            set => this.RaiseAndSetIfChanged(ref _icon, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }
    }
} 