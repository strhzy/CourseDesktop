using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace DnDPartyManager.M
{
    public partial class Combat : ObservableObject
    {
        [ObservableProperty]
        private string name;
        
        [ObservableProperty]
        private ObservableCollection<CombatParticipant> participants = new();
        
        [ObservableProperty]
        private int currentRound = 1;
        
        [ObservableProperty]
        private int currentTurnIndex = 0;
        
        [BsonIgnore]
        [ObservableProperty]
        private CombatParticipant currentParticipant;
    }

    public enum ParticipantType
    {
        Player,
        Npc,
        Enemy
    }
}