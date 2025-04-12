namespace DnDPartyManager.VM;

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System.Collections.ObjectModel;
using System.ComponentModel;

public partial class CharacterPageViewModel : ObservableObject
{
    private static readonly ILiteCollection<PlayerCharacter> col = DBHelper.DB.GetCollection<PlayerCharacter>("characters");

    [ObservableProperty]
    private PlayerCharacter _playerCharacter;

    [ObservableProperty]
    private ObservableCollection<PlayerCharacter> _playerCharacters;

    public CharacterPageViewModel()
    {
        _playerCharacters = UnivHelper.ListToObserv(col.FindAll().ToList());
        if (Application.Current?.Properties["SelectedPlayer"] is PlayerCharacter selectedPlayer)
        {
            PlayerCharacter = selectedPlayer;
        }
        else
        {
            PlayerCharacter = col.FindAll().FirstOrDefault() ?? new PlayerCharacter();
        }
        SubscribeToPlayerCharacterChanges();
    }

    private void SubscribeToPlayerCharacterChanges()
    {
        if (_playerCharacter != null)
        {
            _playerCharacter.PropertyChanged += OnPlayerCharacterPropertyChanged;
        }
    }

    private void OnPlayerCharacterPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        col.Update(_playerCharacter);
        UpdatePlayerCharacters();
    }

    partial void OnPlayerCharacterChanged(PlayerCharacter oldValue, PlayerCharacter newValue)
    {
        if (oldValue != null)
        {
            oldValue.PropertyChanged -= OnPlayerCharacterPropertyChanged;
        }

        if (newValue != null)
        {
            newValue.PropertyChanged += OnPlayerCharacterPropertyChanged;
        }

        col.Update(newValue);
        UpdatePlayerCharacters();
    }

    private void UpdatePlayerCharacters()
    {
        _playerCharacters = UnivHelper.ListToObserv(col.FindAll().ToList());
        OnPropertyChanged(nameof(PlayerCharacters));
    }
}