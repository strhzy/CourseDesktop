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
    
    [ObservableProperty] private int currentPage = 1;
    
    [ObservableProperty] private ObservableCollection<Enemy> enemies = new ObservableCollection<Enemy>();

    [RelayCommand]
    private async Task LoadEnemies()
    {
        Enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync())
        {
            Enemies.Add(enem);
            Console.WriteLine(Enemies.Count);
        }
    }

    [RelayCommand]
    private async Task NextPage()
    {
        CurrentPage++;
        Enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync(CurrentPage))
        {
            Enemies.Add(enem);
            Console.WriteLine(Enemies.Count);
        }
    }
    
    [RelayCommand]
    private async Task PrevPage()
    {
        CurrentPage--;
        Enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync(CurrentPage))
        {
            Application.Current.Dispatcher.Invoke(() => Enemies.Add(enem));
            Console.WriteLine(Enemies.Count);
        }
    }

    public EnemiesViewModel()
    {
        Application.Current.Dispatcher.InvokeAsync(async () => await LoadEnemies());
    }
}