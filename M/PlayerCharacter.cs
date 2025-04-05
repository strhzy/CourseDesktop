using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class PlayerCharacter : Character
    {
        [ObservableProperty]
        private string playerName = string.Empty;
        
        [ObservableProperty]
        private string personalityTraits = string.Empty;

        [ObservableProperty]
        private string ideals = string.Empty;

        [ObservableProperty]
        private string bonds = string.Empty;

        [ObservableProperty]
        private string flaws = string.Empty;
        
        [ObservableProperty]
        private int coppers = 0;
        
        [ObservableProperty]
        private int silvers = 0;
        
        [ObservableProperty]
        private int gold = 0;
        
        [ObservableProperty]
        private int electrums = 0;
        
        [ObservableProperty]
        private int platinums = 0;
    }
}