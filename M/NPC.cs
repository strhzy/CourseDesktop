using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class NPC : ObservableObject
    {
        [BsonId]
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string race = string.Empty;

        [ObservableProperty]
        private string occupation = string.Empty;

        [ObservableProperty]
        private int hitPoints = 10;

        [ObservableProperty]
        private int armorClass = 10;

        [ObservableProperty]
        private string personalityTraits = string.Empty;

        [ObservableProperty]
        private string ideals = string.Empty;

        [ObservableProperty]
        private string bonds = string.Empty;

        [ObservableProperty]
        private string flaws = string.Empty;
    }
}