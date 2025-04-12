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
        private ObservableCollection<int> playersIds = new();

        [ObservableProperty]
        private ObservableCollection<int> npcsIds = new();

        [ObservableProperty]
        private ObservableCollection<Enemy> enemies = new();
    }
}