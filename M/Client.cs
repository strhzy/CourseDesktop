using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M;

public partial class Client : ObservableObject
{
    [ObservableProperty] private string name;
    [ObservableProperty] private string ip;
    [ObservableProperty]
    private DateTime connectedTime = DateTime.Now;
}