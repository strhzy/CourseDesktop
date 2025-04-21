using System.Net.WebSockets;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.M;

public partial class Client : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _ip;
    [ObservableProperty] private DateTime _connectedTime = DateTime.Now;
    
    public WebSocket Socket { get; set; } // Добавляем ссылку на соединение
}