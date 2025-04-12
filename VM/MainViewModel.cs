using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;
using DnDPartyManager.S;

namespace DnDPartyManager.VM;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Tab> tabs;
    
    [ObservableProperty]
    private Tab? selectedTab;
    public MainViewModel()
    {
        tabs =
        [
            new Tab() { Name = "Кампания", Uri = new Uri("/V/CampaignPage.xaml", UriKind.Relative) },
            new Tab() { Name = "Монстры", Uri = new Uri("/V/EnemiesPage.xaml", UriKind.Relative) },
            new Tab() { Name = "Персонажи игроков", Uri = new Uri("/V/PlayerCharacterPage.xaml", UriKind.Relative) },
            new Tab() { Name = "НИПы", Uri = new Uri("/V/NPCPage.xaml", UriKind.Relative) }
        ];
        Thread.Sleep(10);
        SelectedTab = new Tab();
        SelectedTab = tabs[0];
    }
}