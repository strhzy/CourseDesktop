using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;

namespace DnDPartyManager.VM;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private Object? selectedItem;
    [ObservableProperty] private Combat? selectedCombat;
    [ObservableProperty] private StoryElement? selectedStory;

    public SettingsViewModel()
    {
        selectedItem = Application.Current.Properties["SelectedPlotItem"];
        if (selectedItem != null)
        {
            if (selectedItem is Combat)
            {
                selectedCombat = selectedItem as Combat;
            }
            else if (selectedItem is StoryElement)
            {
                selectedStory = selectedItem as StoryElement;
            }
        }
    }
}