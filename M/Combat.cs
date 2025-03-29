using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class Combat : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private List<PlayerCharacter> players = new();

        [ObservableProperty]
        private List<NPC> npcs = new();

        [ObservableProperty]
        private List<Enemy> enemies = new();
    }
}