using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;
using System.Collections.Generic;

namespace DnDPartyManager.M
{
    public partial class Character : ObservableObject
    {
        [ObservableProperty]
        [BsonId(true)]
        private int id;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string classType = string.Empty;

        [ObservableProperty]
        private string race = string.Empty;

        [ObservableProperty]
        private string background = string.Empty;

        [ObservableProperty]
        private string alignment = string.Empty;

        [ObservableProperty]
        private int experiencePoints;

        [ObservableProperty]
        private int level = 1;

        [ObservableProperty]
        private int strength;

        [ObservableProperty]
        private int dexterity;

        [ObservableProperty]
        private int constitution;

        [ObservableProperty]
        private int intelligence;

        [ObservableProperty]
        private int wisdom;

        [ObservableProperty]
        private int charisma;

        [ObservableProperty]
        private bool inspiration;

        [ObservableProperty]
        private int proficiencyBonus;

        // Спасброски
        [ObservableProperty]
        private int savingThrowStrength;

        [ObservableProperty]
        private int savingThrowDexterity;

        [ObservableProperty]
        private int savingThrowConstitution;

        [ObservableProperty]
        private int savingThrowIntelligence;

        [ObservableProperty]
        private int savingThrowWisdom;

        [ObservableProperty]
        private int savingThrowCharisma;

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

        // Навыки
        [ObservableProperty]
        private int acrobatics;

        [ObservableProperty]
        private int animalHandling;

        [ObservableProperty]
        private int arcana;

        [ObservableProperty]
        private int athletics;

        [ObservableProperty]
        private int deception;

        [ObservableProperty]
        private int history;

        [ObservableProperty]
        private int insight;

        [ObservableProperty]
        private int intimidation;

        [ObservableProperty]
        private int investigation;

        [ObservableProperty]
        private int medicine;

        [ObservableProperty]
        private int nature;

        [ObservableProperty]
        private int perception;

        [ObservableProperty]
        private int performance;

        [ObservableProperty]
        private int persuasion;

        [ObservableProperty]
        private int religion;

        [ObservableProperty]
        private int sleightOfHand;

        [ObservableProperty]
        private int stealth;

        [ObservableProperty]
        private int survival;

        [ObservableProperty]
        private int armorClass;

        [ObservableProperty]
        private int initiative;

        [ObservableProperty]
        private int speed;

        [ObservableProperty]
        private int maxHitPoints;

        [ObservableProperty]
        private int currentHitPoints;

        [ObservableProperty]
        private int temporaryHitPoints;

        [ObservableProperty]
        private string hitDice = string.Empty;

        [ObservableProperty]
        private int deathSaveSuccesses;

        [ObservableProperty]
        private int deathSaveFailures;

        [ObservableProperty]
        private int passiveWisdom;

        [ObservableProperty]
        private string proficienciesAndLanguages = string.Empty;

        [ObservableProperty]
        private int copperPieces;

        [ObservableProperty]
        private int silverPieces;

        [ObservableProperty]
        private int goldPieces;

        [ObservableProperty]
        private int electrumPieces;

        [ObservableProperty]
        private int platinumPieces;

        [ObservableProperty]
        private string equipment = string.Empty;

        [ObservableProperty]
        private string featuresAndTraits = string.Empty;

        [ObservableProperty]
        private List<Attack> attacks = new List<Attack>();

        // Свойство для отображения HP в бою
        public int Hp
        {
            get => CurrentHitPoints;
            set => CurrentHitPoints = value;
        }

        // Логика расчета спасбросков
        private int CalculateSavingThrow(int abilityScore, bool isProficient)
        {
            int modifier = (abilityScore - 10) / 2;
            return isProficient ? modifier + ProficiencyBonus : modifier;
        }

        partial void OnStrengthChanged(int value) => SavingThrowStrength = CalculateSavingThrow(value, SavingThrowStrengthProficiency);
        partial void OnDexterityChanged(int value) => SavingThrowDexterity = CalculateSavingThrow(value, SavingThrowDexterityProficiency);
        partial void OnConstitutionChanged(int value) => SavingThrowConstitution = CalculateSavingThrow(value, SavingThrowConstitutionProficiency);
        partial void OnIntelligenceChanged(int value) => SavingThrowIntelligence = CalculateSavingThrow(value, SavingThrowIntelligenceProficiency);
        partial void OnWisdomChanged(int value) => SavingThrowWisdom = CalculateSavingThrow(value, SavingThrowWisdomProficiency);
        partial void OnCharismaChanged(int value) => SavingThrowCharisma = CalculateSavingThrow(value, SavingThrowCharismaProficiency);

        partial void OnProficiencyBonusChanged(int value)
        {
            SavingThrowStrength = CalculateSavingThrow(Strength, SavingThrowStrengthProficiency);
            SavingThrowDexterity = CalculateSavingThrow(Dexterity, SavingThrowDexterityProficiency);
            SavingThrowConstitution = CalculateSavingThrow(Constitution, SavingThrowConstitutionProficiency);
            SavingThrowIntelligence = CalculateSavingThrow(Intelligence, SavingThrowIntelligenceProficiency);
            SavingThrowWisdom = CalculateSavingThrow(Wisdom, SavingThrowWisdomProficiency);
            SavingThrowCharisma = CalculateSavingThrow(Charisma, SavingThrowCharismaProficiency);
        }

        partial void OnSavingThrowStrengthProficiencyChanged(bool value) => SavingThrowStrength = CalculateSavingThrow(Strength, value);
        partial void OnSavingThrowDexterityProficiencyChanged(bool value) => SavingThrowDexterity = CalculateSavingThrow(Dexterity, value);
        partial void OnSavingThrowConstitutionProficiencyChanged(bool value) => SavingThrowConstitution = CalculateSavingThrow(Constitution, value);
        partial void OnSavingThrowIntelligenceProficiencyChanged(bool value) => SavingThrowIntelligence = CalculateSavingThrow(Intelligence, value);
        partial void OnSavingThrowWisdomProficiencyChanged(bool value) => SavingThrowWisdom = CalculateSavingThrow(Wisdom, value);
        partial void OnSavingThrowCharismaProficiencyChanged(bool value) => SavingThrowCharisma = CalculateSavingThrow(Charisma, value);
    }
}