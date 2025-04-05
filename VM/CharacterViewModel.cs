using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;

namespace DnDPartyManager.VM;

public partial class CharacterViewModel : ObservableObject
{
    [ObservableProperty] static ILiteCollection<PlayerCharacter> col = DBHelper.DB.GetCollection<PlayerCharacter>("campaigns");
    [ObservableProperty] private ObservableCollection<PlayerCharacter> playerCharacters;
    [ObservableProperty] private PlayerCharacter selectedPlayer;
    
    public CharacterViewModel()
    {
        playerCharacters = UnivHelper.ListToObserv(col.FindAll().ToList());
        
    }
}