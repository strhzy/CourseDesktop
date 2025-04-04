using System.Windows.Controls;
using DnDPartyManager.VM;

namespace DnDPartyManager.V.UserControls;

public partial class StorySettings : Page
{
    public StorySettings()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}