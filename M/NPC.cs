using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class NPC : Character
    {
        [ObservableProperty]
        private string description = string.Empty;
    }
}