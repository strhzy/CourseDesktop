using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DnDPartyManager.VM;

public partial class CharacterViewModel : ObservableObject
{
    [ObservableProperty] static ILiteCollection<PlayerCharacter> col = DBHelper.DB.GetCollection<PlayerCharacter>("characters");
    [ObservableProperty] private ObservableCollection<PlayerCharacter> playerCharacters;
    [ObservableProperty] private PlayerCharacter selectedPlayer;
    [ObservableProperty] private Uri uri;
    
    public CharacterViewModel()
    {
        UpdateCol();
    }

    [RelayCommand]
    public void AddCharacter()
    {
        col.Insert(new PlayerCharacter(){Name = "Новый персонаж"});
        UpdateCol();
    }
    
    [RelayCommand]
    private void DeleteItem(PlayerCharacter item)
    {
        if (item != null && PlayerCharacters.Contains(item))
        {
            col.Delete(item.Id);
            Console.WriteLine("Delete " + item.Name);
            UpdateCol();
        }
    }
    
    [RelayCommand]
    private void ExportItem(PlayerCharacter item)
    {
        if (item != null && PlayerCharacters.Contains(item))
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string filename = dialog.FileName;
                string json = JsonSerializer.Serialize(item);
                string filePath = Path.Combine(filename, "output.json");
                File.WriteAllText(filePath, json);
                MessageBox.Show("Экспорт персонажа " + item.Name + ": успешно");
            }
        }
    }
    
    [RelayCommand]
    private void ImportItem()
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        CommonFileDialogResult result = dialog.ShowDialog();
        if (result == CommonFileDialogResult.Ok)
        {
            string filename = dialog.FileName;
            if (File.Exists(filename) && filename.EndsWith(".json"))
            {
                string json = File.ReadAllText(filename);
                PlayerCharacter item = JsonSerializer.Deserialize<PlayerCharacter>(json);
                item.Id = col.FindAll().Last().Id + 1;
                col.Insert(item);
                MessageBox.Show("Импорт персонажа " + item.Name + ": успешно");
                UpdateCol();
            }
        }
    }

    [RelayCommand]
    private void Clear()
    {
        Uri = null;
    }

    public void UpdateCol()
    {
        if (PlayerCharacters != null)
        {
            if (PlayerCharacters.Count() != 0)
            {
                PlayerCharacters.Clear();
            }
        }
        PlayerCharacters = UnivHelper.ListToObserv(col.FindAll().ToList());
    }
    
    partial void OnSelectedPlayerChanged(PlayerCharacter newValue)
    {
        Uri = new Uri("", UriKind.Relative);
        if (newValue != null)
        {
            Console.WriteLine($"Chose player: {newValue.Name}");
            Console.WriteLine("Cleared uri");
            Application.Current.Properties["SelectedPlayer"] = newValue;
            Uri = new Uri("/V/UserControls/Character.xaml", UriKind.Relative);
            Console.WriteLine($"Set uri: {Uri}");
        }
    }
}