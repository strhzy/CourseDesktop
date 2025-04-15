using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M
{
    public class Attack
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("desc")]
        public string Description { get; set; }

        [JsonPropertyName("attack_bonus")]
        public int? AttackBonus { get; set; }

        [JsonPropertyName("damage_dice")]
        public string DamageDice { get; set; }
    }
}