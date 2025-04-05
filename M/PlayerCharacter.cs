using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DnDPartyManager.M
{
    public partial class PlayerCharacter : Character
    {
        [ObservableProperty]
        private string playerName = string.Empty; // Имя игрока было только в PlayerCharacter

        [ObservableProperty]
        private int copperPieces; // ММ (медные монеты)

        [ObservableProperty]
        private int silverPieces; // СМ (серебряные монеты)

        [ObservableProperty]
        private int electrumPieces; // ЭМ (электрумовые монеты)

        [ObservableProperty]
        private int goldPieces; // ЗМ (золотые монеты)

        [ObservableProperty]
        private int platinumPieces; // ПМ (платиновые монеты)

        [ObservableProperty]
        private string personalityTraits = string.Empty; // Черты характера

        [ObservableProperty]
        private string ideals = string.Empty; // Идеалы

        [ObservableProperty]
        private string bonds = string.Empty; // Привязанности

        [ObservableProperty]
        private string flaws = string.Empty; // Слабости
        
        [ObservableProperty]
        private bool savingThrowStrengthProficiency;

        [ObservableProperty]
        private bool savingThrowDexterityProficiency;

        [ObservableProperty]
        private bool savingThrowConstitutionProficiency;

        [ObservableProperty]
        private bool savingThrowIntelligenceProficiency;

        [ObservableProperty]
        private bool savingThrowWisdomProficiency;

        [ObservableProperty]
        private bool savingThrowCharismaProficiency;
        
        private int CalculateSavingThrow(int abilityScore, bool isProficient)
        {
            int modifier = (abilityScore - 10) / 2;
            return isProficient ? modifier + ProficiencyBonus : modifier;
        }

        [ObservableProperty]
        private int strength;
        partial void OnStrengthChanged(int value) => SavingThrowStrength = CalculateSavingThrow(value, SavingThrowStrengthProficiency);

        [ObservableProperty]
        private int dexterity;
        partial void OnDexterityChanged(int value) => SavingThrowDexterity = CalculateSavingThrow(value, SavingThrowDexterityProficiency);

        [ObservableProperty]
        private int constitution;
        partial void OnConstitutionChanged(int value) => SavingThrowConstitution = CalculateSavingThrow(value, SavingThrowConstitutionProficiency);

        [ObservableProperty]
        private int intelligence;
        partial void OnIntelligenceChanged(int value) => SavingThrowIntelligence = CalculateSavingThrow(value, SavingThrowIntelligenceProficiency);

        [ObservableProperty]
        private int wisdom;
        partial void OnWisdomChanged(int value) => SavingThrowWisdom = CalculateSavingThrow(value, SavingThrowWisdomProficiency);

        [ObservableProperty]
        private int charisma;
        partial void OnCharismaChanged(int value) => SavingThrowCharisma = CalculateSavingThrow(value, SavingThrowCharismaProficiency);

        [ObservableProperty]
        private int proficiencyBonus;
        partial void OnProficiencyBonusChanged(int value)
        {
            SavingThrowStrength = CalculateSavingThrow(Strength, SavingThrowStrengthProficiency);
            SavingThrowDexterity = CalculateSavingThrow(Dexterity, SavingThrowDexterityProficiency);
            SavingThrowConstitution = CalculateSavingThrow(Constitution, SavingThrowConstitutionProficiency);
            SavingThrowIntelligence = CalculateSavingThrow(Intelligence, SavingThrowIntelligenceProficiency);
            SavingThrowWisdom = CalculateSavingThrow(Wisdom, SavingThrowWisdomProficiency);
            SavingThrowCharisma = CalculateSavingThrow(Charisma, SavingThrowCharismaProficiency);
        }
    }
}