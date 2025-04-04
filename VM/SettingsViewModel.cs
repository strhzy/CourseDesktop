using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;

namespace DnDPartyManager.VM;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private Object selectedItem;
    [ObservableProperty] private Combat selectedCombat;
    [ObservableProperty] private StoryElement selectedStory;
    [ObservableProperty] private Campaign currentCampaign;

    public SettingsViewModel()
    {
        currentCampaign = Application.Current.Properties["CurrentCampaign"] as Campaign;
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

    partial void OnSelectedCombatChanged(Combat oldValue, Combat newValue)
    {
        Console.WriteLine($"SelectedCombat изменился: {oldValue?.Name} → {newValue?.Name}");
    }

    partial void OnSelectedStoryChanged(StoryElement oldValue, StoryElement newValue)
    {
        Console.WriteLine($"SelectedStory изменился: {oldValue?.Name} → {newValue?.Name}");
    }
}