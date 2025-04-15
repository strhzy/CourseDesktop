using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace DnDPartyManager.VM
{
    public partial class CharacterPageViewModel : ObservableObject
    {
        private static readonly ILiteCollection<PlayerCharacter> playerCol = DBHelper.DB.GetCollection<PlayerCharacter>("characters");
        private static readonly ILiteCollection<NPC> npcCol = DBHelper.DB.GetCollection<NPC>("npcs");

        [ObservableProperty]
        private Character character;

        [ObservableProperty]
        private ObservableCollection<PlayerCharacter> playerCharacters;

        [ObservableProperty]
        private ObservableCollection<NPC> npcs;

        public CharacterPageViewModel()
        {
            PlayerCharacters = UnivHelper.ListToObserv(playerCol.FindAll().ToList());
            Npcs = UnivHelper.ListToObserv(npcCol.FindAll().ToList());

            if (Application.Current?.Properties["SelectedPlayer"] is PlayerCharacter selectedPlayer)
            {
                Character = selectedPlayer;
            }
            else if (Application.Current?.Properties["SelectedNPC"] is NPC selectedNPC)
            {
                Character = selectedNPC;
            }
            else
            {
                Character = playerCol.FindAll().FirstOrDefault() ?? new PlayerCharacter();
            }

            SubscribeToCharacterChanges();
        }

        private void SubscribeToCharacterChanges()
        {
            if (Character != null)
            {
                Character.PropertyChanged += OnCharacterPropertyChanged;
            }
        }

        private void OnCharacterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Character is PlayerCharacter player)
            {
                playerCol.Update(player);
            }
            else if (Character is NPC npc)
            {
                npcCol.Update(npc);
            }
            UpdateCollections();
        }

        partial void OnCharacterChanged(Character oldValue, Character newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= OnCharacterPropertyChanged;
            }
            if (newValue != null)
            {
                newValue.PropertyChanged += OnCharacterPropertyChanged;
            }

            if (newValue is PlayerCharacter player)
            {
                playerCol.Update(player);
            }
            else if (newValue is NPC npc)
            {
                npcCol.Update(npc);
            }

            UpdateCollections();
        }

        private void UpdateCollections()
        {
            PlayerCharacters = UnivHelper.ListToObserv(playerCol.FindAll().ToList());
            Npcs = UnivHelper.ListToObserv(npcCol.FindAll().ToList());
            OnPropertyChanged(nameof(PlayerCharacters));
            OnPropertyChanged(nameof(Npcs));
        }
    }
}