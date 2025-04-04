using System.Collections.ObjectModel;
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
        col.Insert(new Campaign(){Name = "123"});
        
        Campaigns = UnivHelper.ListToObserv(col.FindAll().ToList());
        Campaign = Campaigns.First();
        
        if (Campaign != null)
        {
            ObservableCollection<Object> plots = new ObservableCollection<Object>();
            plots.Add(new Combat());
            plots.Add(new Combat());
            plots.Add(new Combat());
            plots.Add(new StoryElement());
            plots.Add(new StoryElement());
            Campaign.PlotItems.AddRange(plots);
            PlotItems = UnivHelper.ListToObserv(Campaign.PlotItems.ToList());
        }
    }

    
}