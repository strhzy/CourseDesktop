using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DnDPartyManager.M
{
    public partial class Character : ObservableObject
    {
        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string classType = string.Empty;

        [ObservableProperty]
        private string background = string.Empty;

        [ObservableProperty]
        private string race = string.Empty;

        [ObservableProperty]
        private string alignment = string.Empty;

        [ObservableProperty]
        private int experiencePoints;

        [ObservableProperty]
        private int level = 1;

        // Характеристики
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

        // Производные характеристики
        [ObservableProperty]
        private int inspiration;

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
        private int passiveWisdom;

        // Боевые характеристики
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

        // Атаки и заклинания
        [ObservableProperty]
        private List<Attack> attacks = new();

        // Особенности и черты
        [ObservableProperty]
        private List<string> featuresAndTraits = new();

        // Снаряжение
        [ObservableProperty]
        private List<string> equipment = new();

        // Прочие владения и языки
        [ObservableProperty]
        private List<string> proficienciesAndLanguages = new();
    }
}