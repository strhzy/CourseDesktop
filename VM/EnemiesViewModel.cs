using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;

namespace DnDPartyManager.VM;

public partial class EnemiesViewModel : ObservableObject
{
    [ObservableProperty] private List<string> slugs;
    
    ILiteCollection<Enemy> enemy_col = DBHelper.DB.GetCollection<Enemy>("enemies");

    [ObservableProperty] private Enemy enemy;
    
    [ObservableProperty] private ObservableCollection<Enemy> enemies = new ObservableCollection<Enemy>();

    [RelayCommand]
    private async Task LoadEnemies()
    {
        enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync()) enemies.Add(enem);
    }

    public EnemiesViewModel()
    {
        
    }
}