using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class StoryElement : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;
    }
}