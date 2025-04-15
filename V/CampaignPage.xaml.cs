using System.Windows.Controls;
using DnDPartyManager.VM;

namespace DnDPartyManager.V;

public partial class CampaignPage : Page
{
    public CampaignPage()
    {
        InitializeComponent();
        DataContext = new CampaignViewModel();
    }
}