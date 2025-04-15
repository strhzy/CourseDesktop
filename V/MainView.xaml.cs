﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DnDPartyManager.VM;

namespace DnDPartyManager.V;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}