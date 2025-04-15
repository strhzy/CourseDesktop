using System;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System.Linq;

namespace DnDPartyManager.VM
{
    public partial class NPCViewModel : ObservableObject
    {
        private static readonly ILiteCollection<Campaign> campaignCol = DBHelper.DB.GetCollection<Campaign>("campaigns");
        private static readonly ILiteCollection<NPC> col = DBHelper.DB.GetCollection<NPC>("Npcs");

        [ObservableProperty]
        private ObservableCollection<NPC> npcs;

        [ObservableProperty]
        private NPC selectedNPC;

        [ObservableProperty]
        private Uri uri;

        [ObservableProperty]
        private ObservableCollection<Combat> combats;

        [ObservableProperty]
        private Combat selectedCombat;

        public NPCViewModel()
        {
            UpdateCol();
            LoadCombats();
            SubscribeToCampaignChanges();
        }

        private void SubscribeToCampaignChanges()
        {
            var campaigns = campaignCol.FindAll().ToList();
            foreach (var campaign in campaigns)
            {
                campaign.PlotItems.CollectionChanged += (s, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() => LoadCombats());
                };
            }
        }

        private void LoadCombats()
        {
            var campaign = campaignCol.FindAll().FirstOrDefault();
            if (campaign != null)
            {
                var activeCombats = campaign.PlotItems.OfType<Combat>().Where(c => !c.IsFinished).ToList();
                Combats = new ObservableCollection<Combat>(activeCombats);
                if (Combats.Any() && SelectedCombat == null)
                {
                    SelectedCombat = Combats.First();
                }
            }
            else
            {
                Combats = new ObservableCollection<Combat>();
            }
        }

        [RelayCommand]
        public void AddNPC()
        {
            col.Insert(new NPC { Name = "Новый НИП", Description = "Описание НИПа" });
            UpdateCol();
        }

        [RelayCommand]
        private void DeleteItem(NPC item)
        {
            if (item != null && Npcs.Contains(item))
            {
                col.Delete(item.Id);
                Console.WriteLine("Delete " + item.Name);
                UpdateCol();
            }
        }

        [RelayCommand]
        private void Clear()
        {
            Uri = null;
        }

        [RelayCommand]
        private void AddToCombat()
        {
            if (SelectedNPC == null)
            {
                MessageBox.Show("Пожалуйста, выберите НИПа.");
                return;
            }

            if (SelectedCombat == null)
            {
                MessageBox.Show("Пожалуйста, выберите бой.");
                return;
            }

            if (SelectedCombat.Participants.Values.Any(p => p.Id == SelectedNPC.Id))
            {
                MessageBox.Show("Этот НИП уже добавлен в бой.");
                return;
            }

            int initiative = SelectedNPC.Initiative;
            while (SelectedCombat.Participants.ContainsKey(initiative) || SelectedCombat.Enemies.ContainsKey(initiative))
            {
                initiative++;
            }
            SelectedNPC.Initiative = initiative;

            SelectedCombat.Participants[SelectedNPC.Initiative] = SelectedNPC;
            col.Update(SelectedNPC);

            var campaign = campaignCol.FindAll().FirstOrDefault();
            if (campaign != null)
            {
                campaignCol.Update(campaign);
            }

            MessageBox.Show($"{SelectedNPC.Name} добавлен в бой {SelectedCombat.Name}.");
        }

        public void UpdateCol()
        {
            if (Npcs != null)
            {
                if (Npcs.Count() != 0)
                {
                    Npcs.Clear();
                }
            }
            Npcs = UnivHelper.ListToObserv(col.FindAll().ToList());
        }

        partial void OnSelectedNPCChanged(NPC newValue)
        {
            Uri = new Uri("", UriKind.Relative);
            if (newValue != null)
            {
                Console.WriteLine($"Chose NPC: {newValue.Name}");
                Application.Current.Properties["SelectedNPC"] = newValue;
                Uri = new Uri("/V/UserControls/Character.xaml", UriKind.Relative);
                Console.WriteLine($"Set uri: {Uri}");
            }
        }
    }
}