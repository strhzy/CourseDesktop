using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
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
            new Tab() { Name = "Монстры", Uri = new Uri("/V/EnemiesPage.xaml", UriKind.Relative) }
        ];
        selectedTab = tabs[0];
    }
}

public partial class Tab
{
    public string Name { get; set; }
    public Uri Uri { get; set; }
}