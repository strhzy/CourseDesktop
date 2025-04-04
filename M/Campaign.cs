using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Campaign : ObservableObject
    {
        [BsonId(true)]
        [ObservableProperty]
        private long id;
        
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private List<Object> plotItems = new();
    }
}