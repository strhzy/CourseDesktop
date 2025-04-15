using System;
using DnDPartyManager.M;

namespace DnDPartyManager.S;

using System.Windows;
using System.Windows.Controls;

public class ElementTemplateSelector : DataTemplateSelector
{
    public DataTemplate CombatTemplate { get; set; }
    public DataTemplate StoryElementTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is Combat)
        {
            Console.WriteLine("Combat item selected");
            return CombatTemplate;
        }
        if (item is StoryElement)
        {
            Console.WriteLine("StoryElement item selected");
            return StoryElementTemplate;
        }

        Console.WriteLine("Unknown item selected");
        return base.SelectTemplate(item, container);
    }
}
