using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        UpdateCol();
        
    }

    [RelayCommand]
    public void AddCharacter()
    {
        col.Insert(new PlayerCharacter(){Name = "Новый персонаж"});
        UpdateCol();
    }
    
    [RelayCommand]
    private void DeleteItem(PlayerCharacter item)
    {
        if (item != null && PlayerCharacters.Contains(item))
        {
            col.Delete(item.Id);
            Console.WriteLine("Delete " + item.Name);
            UpdateCol();
        }
    }

    public void UpdateCol()
    {
        playerCharacters.Clear();
        playerCharacters = UnivHelper.ListToObserv(col.FindAll().ToList());
    }
    
    partial void OnSelectedPlayerChanged(PlayerCharacter newValue)
    {
        if (newValue != null)
        {
            Console.WriteLine($"Chose player: {newValue.Name}");
            Application.Current.Properties["SelectedPlayer"] = newValue;
            
        }
    }
}