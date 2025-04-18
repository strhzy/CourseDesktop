using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;
using LiteDB;

namespace DnDPartyManager.S;

public partial class CombatAddHelper : ObservableObject
{
    private static readonly ILiteCollection<Campaign> col = DBHelper.DB.GetCollection<Campaign>("campaigns");
    
    [ObservableProperty] private Campaign selectedCampaign;
    
    [ObservableProperty] private ObservableCollection<Combat> combats;

    public CombatAddHelper()
    {
        if (Application.Current.Properties["CurrentCampaign"] != null)
        {
            selectedCampaign = Application.Current.Properties["CurrentCampaign"] as Campaign;
        }
    }

    public void AddParticipant(object item, Combat combat)
    {
        
    }
}