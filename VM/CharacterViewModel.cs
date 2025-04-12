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
    [ObservableProperty] static ILiteCollection<PlayerCharacter> col = DBHelper.DB.GetCollection<PlayerCharacter>("characters");
    [ObservableProperty] private ObservableCollection<PlayerCharacter> playerCharacters;
    [ObservableProperty] private PlayerCharacter selectedPlayer;
    [ObservableProperty] private Uri uri;
    
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

    [RelayCommand]
    private void Clear()
    {
        Uri = null;
    }

    public void UpdateCol()
    {
        if (PlayerCharacters != null)
        {
            if (PlayerCharacters.Count() != 0)
            {
                PlayerCharacters.Clear();
            }
        }
        PlayerCharacters = UnivHelper.ListToObserv(col.FindAll().ToList());
    }
    
    partial void OnSelectedPlayerChanged(PlayerCharacter newValue)
    {
        Uri = new Uri("", UriKind.Relative);
        if (newValue != null)
        {
            Console.WriteLine($"Chose player: {newValue.Name}");
            Console.WriteLine("Cleared uri");
            Application.Current.Properties["SelectedPlayer"] = newValue;
            Uri = new Uri("/V/UserControls/Character.xaml", UriKind.Relative);
            Console.WriteLine($"Set uri: {Uri}");
        }
    }
}