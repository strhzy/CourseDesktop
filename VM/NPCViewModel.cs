using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System.Collections.ObjectModel;
using System.Windows;

namespace DnDPartyManager.VM
{
    public partial class NPCViewModel : ObservableObject
    {
        private static readonly ILiteCollection<NPC> col = DBHelper.DB.GetCollection<NPC>("npcs");

        [ObservableProperty]
        private ObservableCollection<NPC> npcs;

        [ObservableProperty]
        private NPC selectedNPC;

        [ObservableProperty]
        private Uri uri;

        public NPCViewModel()
        {
            LoadNPCs();
        }

        private void LoadNPCs()
        {
            Npcs = new ObservableCollection<NPC>(col.FindAll());
        }

        [RelayCommand]
        private void AddNPC()
        {
            var newNPC = new NPC { Name = "Новый NPC" };
            col.Insert(newNPC);
            LoadNPCs();
            SelectedNPC = newNPC;
        }

        [RelayCommand]
        private void DeleteNPC(NPC npc)
        {
            if (npc != null && MessageBox.Show($"Удалить NPC {npc.Name}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                col.Delete(npc.Id);
                LoadNPCs();
                SelectedNPC = null;
            }
        }

        [RelayCommand]
        private void SaveNPC()
        {
            if (SelectedNPC != null)
            {
                col.Update(SelectedNPC);
                LoadNPCs();
                MessageBox.Show("Изменения сохранены", "Успех");
            }
        }

        [RelayCommand]
        private void ClearSelection()
        {
            SelectedNPC = null;
            Uri = null;
        }

        [RelayCommand]
        private void AddNPCToCombat(NPC npc)
        {
            if (Application.Current.Properties["CurrentCampaign"] is Campaign campaign &&
                Application.Current.Properties["SelectedPlotItem"] is Combat combat)
            {
                var mainViewModel = (Application.Current.MainWindow.DataContext as MainViewModel);
                var campaignVM = mainViewModel?.GetCampaignViewModel();
                
                campaignVM?.AddNPCToCombatCommand.Execute(npc);
            }
            else
            {
                MessageBox.Show("Сначала выберите или создайте бой в кампании");
            }
        }
        
        [RelayCommand]
        private void LoadNPCDetails()
        {
            if (SelectedNPC != null)
            {
                Uri = new Uri("/V/UserControls/NPCDetails.xaml", UriKind.Relative);
            }
            else
            {
                Uri = null;
            }
        }

        partial void OnSelectedNPCChanged(NPC oldValue, NPC newValue)
        {
            if (newValue != null)
            {
                Application.Current.Properties["SelectedNPC"] = newValue;
                LoadNPCDetails();
            }
            else
            {
                Uri = null;
            }
        }
    }
}