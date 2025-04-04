using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class StoryElement : ObservableObject
    {
        [BsonId(true)]
        [ObservableProperty]
        private int id;
        
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;
    }
}