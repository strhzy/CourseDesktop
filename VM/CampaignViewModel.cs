using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;

namespace DnDPartyManager.VM;

public partial class CampaignViewModel : ObservableObject
{
    ILiteCollection<Campaign> col = DBHelper.DB.GetCollection<Campaign>("campaigns");
    partial void OnTitleChanged(string value);
    partial void OnDescriptionChanged(string value);
    
    [ObservableProperty]
    List<Campaign> campaigns;
    
    [ObservableProperty]
    Campaign campaign;
    
    public CampaignViewModel()
    {
        Load();
    }

    partial void OnTitleChanged(string value)
    {
        Console.WriteLine($"Title changed to: {value}");
        Save();
    }
    
    partial void OnDescriptionChanged(string value)
    {
        Console.WriteLine($"Title changed to: {value}");
        Save();
    }
    
    private void Save()
    {
        using var db = new LiteDatabase("data.db");
        var collection = db.GetCollection<Campaign>("campaigns");
        collection.Upsert(Campaign);
    }

    private void Load()
    {
        campaigns = col.FindAll().ToList();
    }
    
    
}