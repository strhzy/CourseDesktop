using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class Enemy : Character
    {
        [ObservableProperty]
        private string type = string.Empty;
    }
}