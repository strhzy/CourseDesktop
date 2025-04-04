using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;

namespace DnDPartyManager.VM;

public partial class CampaignViewModel : ObservableObject
{
    static ILiteCollection<Campaign> col = DBHelper.DB.GetCollection<Campaign>("campaigns");
    
    [ObservableProperty]
    ObservableCollection<Campaign> campaigns;
    
    [ObservableProperty] ObservableCollection<Object> plotItems;

    [ObservableProperty] private Object selectedItem;
    
    [ObservableProperty] private Tab selectedTab;
    
    [ObservableProperty] private ObservableCollection<Tab> tabs;
    
    [ObservableProperty] Campaign campaign;
    
    public CampaignViewModel()
    {
        plotItems = new ObservableCollection<Object>();
        tabs =
        [
            new Tab(){Name = "Сюжет", Uri = new Uri("/V/UserControls/StorySettings.xaml", UriKind.Relative) },
            new Tab(){Name = "Бой", Uri = new Uri("/V/UserControls/CombatSettings.xaml", UriKind.Relative) }
        ];
        Load();
    }
    
    private void Save()
    {
        col.Upsert(Campaign);
    }

    private void Load()
    {
        Campaigns = UnivHelper.ListToObserv(col.FindAll().ToList());
        if (Campaigns.Count == 0)
        {
            col.Insert(new Campaign(){Name = "Кампания 1"});
            Campaigns = UnivHelper.ListToObserv(col.FindAll().ToList());
        }
        Campaign = Campaigns.First();
    }

    [RelayCommand]
    private void UpdateCampaign()
    {
        col.Update(Campaign);
    }

    [RelayCommand]
    private void AddStory()
    {
        StoryElement storyElement = new StoryElement(){Name = "stub"};
        PlotItems.Add(storyElement);
        Campaign.PlotItems = PlotItems;
        Console.WriteLine("Add story " + storyElement.Name);
        UpdateCampaign();
    }

    [RelayCommand]
    private void AddCombat()
    {
        PlotItems.Add(new Combat(){Name = "stub"});
        Campaign.PlotItems = PlotItems;
        Console.WriteLine("Add stub combat");
        UpdateCampaign();
    }
    
    [RelayCommand]
    private void DeleteItem(object item)
    {
        if (item != null && Campaign.PlotItems.Contains(item))
        {
            Campaign.PlotItems.Remove(item);
            UpdateCampaign();
        }
    }

    // partial void OnCampaignChanged(Campaign value)
    // {
    //     Console.WriteLine(Campaign.Name);
    // }
    //
    // partial void OnSelectedItemChanged(object oldValue, object newValue)
    // {
    //     if (newValue != null)
    //     {
    //         Console.WriteLine($"Choosed: {newValue}");
    //         
    //         if (newValue is Combat combat)
    //         {
    //             Console.WriteLine($"Choosed Combat: {combat.Name}");
    //             Application.Current.Properties["SelectedPlotItem"] = selectedItem as Combat;
    //             selectedTab = tabs[1];
    //         }
    //         else if (newValue is StoryElement story)
    //         {
    //             Console.WriteLine($"Choosed StoryElement: {story.Name}");
    //             Application.Current.Properties["SelectedPlotItem"] = selectedItem as StoryElement;
    //             selectedTab = tabs[0];
    //         }
    //     }
    // }
}