using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class Attack : ObservableObject
    {
        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;
    }
}