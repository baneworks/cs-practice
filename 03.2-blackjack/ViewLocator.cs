using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Blackjack.ViewModels;

namespace Blackjack;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        // Type? type;

        //! fixme: seems dirty
        var name = data.GetType().FullName;

        if (name!.Contains("CardForCanvasViewModel"))
            name = name.Replace("CardForCanvasViewModel", "CardViewModel");

        name = name.Replace("ViewModel", "View");

        // var name = data.GetType().FullName!.Replace("ViewModel", "View");

        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}
