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
    
    [ObservableProperty]
    Campaign campaign;
    
    public CampaignViewModel()
    {
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
        campaign.PlotItems.Add(new StoryElement(){Name = "stub", Description = "stub"});
        UpdateCampaign();
    }

    [RelayCommand]
    private void AddCombat()
    {
        Campaign.PlotItems.Add(new Combat(){Name = "stub"});
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

    partial void OnCampaignChanged(Campaign value)
    {
        MessageBox.Show(Campaign.Name);
    }
}