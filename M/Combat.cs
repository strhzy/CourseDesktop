using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DnDPartyManager.M
{
    public partial class Combat : ObservableObject
    {
        [ObservableProperty]
        private string name = "Новый бой";

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private Dictionary<int, Character> participants = new();

        [ObservableProperty]
        private Dictionary<int, Enemy> enemies = new();

        [ObservableProperty]
        private int currentTurn;

        [ObservableProperty]
        private int turnCounter;

        [ObservableProperty]
        private bool isFinished;
    }
}