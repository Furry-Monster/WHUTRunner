using Avalonia.Controls;
using Avalonia.Input;
using WHUTRunner.ViewModels;

namespace WHUTRunner.Views
{
    public partial class MainWindow : Window
    {
        // 主窗口构造函数
        public MainWindow()
        {
            InitializeComponent();

            // 为标题栏添加拖动支持
            var titleBar = this.FindControl<Grid>("TitleBar");
            if (titleBar != null)
            {
                titleBar.PointerPressed += (s, e) =>
                {
                    if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                    {
                        BeginMoveDrag(e);
                    }
                };
            }
        }
    }
}