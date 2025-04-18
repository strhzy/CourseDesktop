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
    
    [ObservableProperty] private string search = "";
    
    [ObservableProperty] private int currentPage = 1;
    
    [ObservableProperty] private ObservableCollection<Enemy> enemies = new ObservableCollection<Enemy>();

    [RelayCommand]
    private async void LoadEnemies()
    {
        Enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync(CurrentPage, Search))
        {
            Enemies.Add(enem);
            Console.WriteLine(Enemies.Count);
        }
    }

    [RelayCommand]
    private async void NextPage()
    {
        CurrentPage++;
        Enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync(CurrentPage, Search))
        {
            Enemies.Add(enem);
            Console.WriteLine(Enemies.Count);
        }
    }
    
    [RelayCommand]
    private async void PrevPage()
    {
        CurrentPage--;
        Enemies.Clear();
        foreach (var enem in await DBHelper.GetEnemiesAsync(CurrentPage, Search))
        {
            Application.Current.Dispatcher.Invoke(() => Enemies.Add(enem));
            Console.WriteLine(Enemies.Count);
        }
    }
    
    [RelayCommand]
    private void AddToCombat(Enemy enemy)
    {
        if (Application.Current.Properties["CurrentCampaign"] is Campaign campaign && 
            Application.Current.Properties["SelectedPlotItem"] is Combat combat)
        {
            var mainViewModel = (Application.Current.MainWindow.DataContext as MainViewModel);
            var campaignVM = mainViewModel?.GetCampaignViewModel();
            
            if (campaignVM?.AddEnemyToCombatCommand?.CanExecute(enemy) == true)
            {
                campaignVM.AddEnemyToCombatCommand.Execute(enemy);
            }
            else
            {
                MessageBox.Show("Команда не может быть выполнена в текущем состоянии");
            }
        }
        else
        {
            MessageBox.Show("Сначала выберите или создайте бой в кампании");
        }
    }

    public EnemiesViewModel()
    {
        //Application.Current.Dispatcher.InvokeAsync(async () => await LoadEnemies());
        LoadEnemies();
    }
}