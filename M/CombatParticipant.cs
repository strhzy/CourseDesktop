using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;

public partial class CombatParticipant : ObservableObject
{
    [ObservableProperty]
    private string name;
        
    [ObservableProperty]
    private int initiative;
        
    [ObservableProperty]
    private int currentHitPoints;
        
    [ObservableProperty]
    private int maxHitPoints;
        
    [ObservableProperty]
    private int armorClass;
        
    [ObservableProperty]
    private bool isActive;
        
    [ObservableProperty]
    private ParticipantType type;
        
    [ObservableProperty]
    private object sourceId;
}