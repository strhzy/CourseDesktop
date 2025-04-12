using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Enemy : ObservableObject
    {
        [BsonId] 
        [ObservableProperty] 
        private string id = System.Guid.NewGuid().ToString();

        [ObservableProperty] 
        public string? slug;

        [ObservableProperty] public int initiative;

        [ObservableProperty] 
        public string? name;
        
        [ObservableProperty] 
        public string? desc;
        
        [ObservableProperty] 
        public string? size;

        [ObservableProperty] 
        public string? type;

        [ObservableProperty] 
        public string? subtype;

        [ObservableProperty] 
        public string? group;

        [ObservableProperty] 
        public string? alignment;

        [ObservableProperty] 
        public int? armor_class;

        [ObservableProperty] 
        public string? armor_desc;

        [ObservableProperty] 
        public int? hit_points;

        [ObservableProperty] 
        public string? hit_dice;

        [ObservableProperty] 
        public Dictionary<string, object>? speed;

        [ObservableProperty] 
        public int? strength;

        [ObservableProperty] 
        public int? dexterity;

        [ObservableProperty] 
        public int? constitution;

        [ObservableProperty] 
        public int? intelligence;

        [ObservableProperty] 
        public int? wisdom;

        [ObservableProperty] 
        public int? charisma;

        [ObservableProperty] 
        public int? perception;

        [ObservableProperty] 
        public Dictionary<string, int>? skills;

        [ObservableProperty] 
        public string? damage_vulnerabilities;

        [ObservableProperty] 
        public string? damage_resistances;

        [ObservableProperty] 
        public string? damage_immunities;

        [ObservableProperty] 
        public string? condition_immunities;

        [ObservableProperty] 
        public string? senses;

        [ObservableProperty] 
        public string? languages;

        [ObservableProperty] 
        public string? challenge_rating;

        [ObservableProperty] 
        public double? cr;

        [ObservableProperty] 
        public List<Attack>? actions;

        [ObservableProperty] 
        public List<Attack>? bonus_actions;

        [ObservableProperty] 
        public List<Attack>? reactions;

        [ObservableProperty] 
        public string? legendary_desc;

        [ObservableProperty] 
        public List<Attack>? legendary_actions;

        [ObservableProperty] 
        public List<SpecialAbility>? special_abilities;

        [ObservableProperty] 
        public List<string>? spell_list;
    }
    public partial class SpecialAbility : ObservableObject
    {
        [ObservableProperty] 
        private string? name;

        [ObservableProperty] 
        private string? desc;
    }
}