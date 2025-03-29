using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class Attack : ObservableObject
    {
        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private int attackBonus;

        [ObservableProperty]
        private string damageAndType = string.Empty;
    }
}