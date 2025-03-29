using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class NPC : Character
    {
        [ObservableProperty]
        private string role = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;
    }
}