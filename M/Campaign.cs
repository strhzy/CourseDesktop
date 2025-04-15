using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Campaign : ObservableObject
    {
        [BsonId(true)]
        [ObservableProperty]
        private int id;
        
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<object> plotItems = new();
    }
}