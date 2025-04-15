using System.Windows;
using System.Windows.Controls;

namespace DnDPartyManager.V.UserControls;

public partial class Enemy : UserControl
{
    public Enemy()
    {
        InitializeComponent();
    }

    private void Btn_OnClick(object sender, RoutedEventArgs e)
    {
        if (pnl.Visibility == Visibility.Collapsed)
        {
            pnl.Visibility = Visibility.Visible;
            btn.Content = "свернуть";
        }
        else
        {
            pnl.Visibility = Visibility.Collapsed;
            btn.Content = "развернуть";
        }
    }
}