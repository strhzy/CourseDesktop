using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public partial class PlayerCharacter : Character
    {
        [ObservableProperty]
        private string playerName = string.Empty;

        // Личные черты
        [ObservableProperty]
        private string personalityTraits = string.Empty;

        // Идеалы
        [ObservableProperty]
        private string ideals = string.Empty;

        // Привязанности
        [ObservableProperty]
        private string bonds = string.Empty;

        // Слабости
        [ObservableProperty]
        private string flaws = string.Empty;
    }
}