using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Enemy : ObservableObject
    {
        [BsonId(true)] [ObservableProperty] private string id;

        [ObservableProperty] public string name;

        [ObservableProperty] public string size;

        [ObservableProperty] public int hit_points;

        [ObservableProperty] public int strength;
        [ObservableProperty] public int dexterity;
        [ObservableProperty] public int constitution;
        [ObservableProperty] public int intelligence;
        [ObservableProperty] public int wisdom;
        [ObservableProperty] public int charisma;

        [ObservableProperty] public List<Attack> actions;
    }
}