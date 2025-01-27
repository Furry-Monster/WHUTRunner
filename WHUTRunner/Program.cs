using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace WHUTRunner
{
    internal sealed class Program
    {
        // 程序入口点
        // 在AppMain被调用之前不要使用任何Avalonia或第三方API
        // 因为此时Avalonia还未初始化，可能会导致程序崩溃
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia配置，请勿删除；设计器也会使用此配置
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()  // 检测运行平台
                .WithInterFont()      // 使用Inter字体
                .LogToTrace()         // 启用日志追踪
                .UseReactiveUI();     // 使用ReactiveUI框架
    }
}
