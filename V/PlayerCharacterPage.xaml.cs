using System.Windows.Controls;
using DnDPartyManager.VM;

namespace DnDPartyManager.V;

public partial class PlayerCharacterPage : Page
{
    public PlayerCharacterPage()
    {
        InitializeComponent();
        DataContext = new CharacterViewModel();
    }
}