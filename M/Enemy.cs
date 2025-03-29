using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Enemy : ObservableObject
    {
        [BsonId(true)]
        [ObservableProperty] 
        private int id;
        
        [ObservableProperty]
        public string name;
        
        [ObservableProperty]
        public string size;
        
        [ObservableProperty]
        public int hitPoints;
        
        [ObservableProperty]
        public Dictionary<string, int> stats;
        
        [ObservableProperty] 
        public List<Attack> actions;
    }
}