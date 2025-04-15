using System.Windows;
using System.Windows.Controls;
using DnDPartyManager.S;
using DnDPartyManager.VM;

namespace DnDPartyManager.V;

public partial class EnemiesPage : Page
{
    public EnemiesPage()
    {
        InitializeComponent();
        DataContext = new EnemiesViewModel();
    }
}