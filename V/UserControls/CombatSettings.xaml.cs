using System.Windows.Controls;
using DnDPartyManager.VM;

namespace DnDPartyManager.V.UserControls;

public partial class CombatSettings : Page
{
    public CombatSettings()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }
}