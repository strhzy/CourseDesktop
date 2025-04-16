using System.Windows.Controls;
using System.Windows.Navigation;
using DnDPartyManager.VM;

namespace DnDPartyManager.V
{
    public partial class NPCPage : Page
    {
        public NPCPage()
        {
            InitializeComponent();
            DataContext = new NPCViewModel();
            DetailsFrame.Navigated += OnFrameNavigated;
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page && page.DataContext == null)
            {
                page.DataContext = this.DataContext;
            }
        }
    }
}