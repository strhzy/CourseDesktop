using System.Windows.Controls;
using DnDPartyManager.VM;

namespace DnDPartyManager.V.UserControls;

public partial class Character : Page
{
    public Character()
    {
        InitializeComponent();
        DataContext = new CharacterPageViewModel();
    }
}