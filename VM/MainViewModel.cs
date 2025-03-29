using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDPartyManager.VM;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private List<Tab> tabs;
    
    [ObservableProperty]
    private Tab? selectedTab;
    public MainViewModel()
    {
        tabs =
        [
            new Tab() { Name = "Кампания", Uri = new Uri("/V/CampaignPage.xaml", UriKind.Relative) }
        ];
        selectedTab = tabs[0];
    }
}

public partial class Tab
{
    public string Name { get; set; }
    public Uri Uri { get; set; }
}