using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Combat : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<PlayerCharacter> players = new();

        [ObservableProperty]
        private ObservableCollection<NPC> npcs = new();

        [ObservableProperty]
        private ObservableCollection<Enemy> enemies = new();
    }
}