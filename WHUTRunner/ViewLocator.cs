using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using WHUTRunner.ViewModels;

namespace WHUTRunner
{
    public class ViewLocator : IDataTemplate
    {
        // 根据ViewModel构建对应的View
        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            // 通过替换类名中的"ViewModel"为"View"来获取对应的View类名
            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            // 如果找不到对应的View，返回一个显示错误信息的TextBlock
            return new TextBlock { Text = "Not Found: " + name };
        }

        // 判断数据对象是否适用于此模板
        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
