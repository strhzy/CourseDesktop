using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;

namespace DnDPartyManager.VM;

public partial class EnemiesViewModel : ObservableObject
{
    [ObservableProperty] private List<string> slugs;
    
    ILiteCollection<Enemy> enemy_col = DBHelper.DB.GetCollection<Enemy>("enemies");

    [ObservableProperty] private Enemy enemy;
    
    public EnemiesViewModel()
    {
        
    }
}