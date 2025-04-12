using System.Windows.Controls;
using DnDPartyManager.VM;

namespace DnDPartyManager.V
{
    public partial class NPCPage : Page
    {
        public NPCPage()
        {
            InitializeComponent();
            DataContext = new NPCViewModel();
        }
    }
}