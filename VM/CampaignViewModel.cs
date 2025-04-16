using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace DnDPartyManager.VM
{
    public partial class CampaignViewModel : ObservableObject
    {
        private static readonly ILiteCollection<Campaign> col = DBHelper.DB.GetCollection<Campaign>("campaigns");
        private static readonly ILiteCollection<PlayerCharacter> playerCol = DBHelper.DB.GetCollection<PlayerCharacter>("characters");
        private static readonly ILiteCollection<Enemy> enemyCol = DBHelper.DB.GetCollection<Enemy>("enemies");

        [ObservableProperty]
        private ObservableCollection<Campaign> campaigns;

        [ObservableProperty]
        private object selectedItem;

        [ObservableProperty]
        private Tab selectedTab;

        [ObservableProperty]
        private ObservableCollection<Tab> tabs;

        [ObservableProperty]
        private Campaign campaign;

        // Боевая система
        [ObservableProperty]
        private ObservableCollection<CombatParticipant> combatParticipants = new();

        [ObservableProperty]
        private CombatParticipant currentCombatParticipant;

        [ObservableProperty]
        private int currentCombatRound = 1;

        [ObservableProperty]
        private bool isCombatActive;

        public CampaignViewModel()
        {
            tabs =
            [
                new Tab() { Name = "Сюжет", Uri = new Uri("/V/UserControls/StorySettings.xaml", UriKind.Relative) },
                new Tab() { Name = "Бой", Uri = new Uri("/V/UserControls/CombatSettings.xaml", UriKind.Relative) }
            ];

            Load();
        }

        private void Load()
        {
            Campaigns = UnivHelper.ListToObserv(col.FindAll().ToList());
            if (Campaigns.Count == 0)
            {
                col.Insert(new Campaign() { Name = "Кампания 1" });
                Campaigns = UnivHelper.ListToObserv(col.FindAll().ToList());
            }
            Campaign = Campaigns.First();
            SubscribeToCampaignChanges();
        }

        private void SubscribeToCampaignChanges()
        {
            Campaign.PlotItems.CollectionChanged += (s, e) =>
            {
                col.Update(Campaign);
                if (SelectedItem is Combat combat)
                {
                    UpdateCombatParticipants(combat);
                }
            };

            foreach (var item in Campaign.PlotItems.OfType<Combat>())
            {
                SubscribeToCombatChanges(item);
            }
        }

        private void SubscribeToCombatChanges(Combat combat)
        {
            combat.PropertyChanged += (s, e) => col.Update(Campaign);
            combat.Participants.CollectionChanged += (s, e) =>
            {
                col.Update(Campaign);
                if (e.NewItems != null)
                {
                    foreach (CombatParticipant participant in e.NewItems)
                    {
                        participant.PropertyChanged += (s, e) => col.Update(Campaign);
                    }
                }
            };
        }

        [RelayCommand]
        private void UpdateCampaign()
        {
            col.Update(Campaign);
        }

        [RelayCommand]
        private void AddStory()
        {
            var storyElement = new StoryElement() { Name = "Новый сюжетный элемент" };
            Campaign.PlotItems.Add(storyElement);
            UpdateCampaign();
        }

        [RelayCommand]
        private void AddCombat()
        {
            var combat = new Combat() { Name = "Новый бой" };
            Campaign.PlotItems.Add(combat);
            SubscribeToCombatChanges(combat);
            UpdateCampaign();
        }

        [RelayCommand]
        private void DeleteItem(object item)
        {
            if (item != null && Campaign.PlotItems.Contains(item))
            {
                Campaign.PlotItems.Remove(item);
                UpdateCampaign();
            }
        }

        [RelayCommand]
        private void StartCombat()
        {
            if (SelectedItem is Combat combat && combat.Participants.Any())
            {
                combat.CurrentRound = 1;
                combat.CurrentTurnIndex = 0;
                combat.CurrentParticipant = combat.Participants[0];
                combat.CurrentParticipant.IsActive = true;
                IsCombatActive = true;
                
                CurrentCombatRound = combat.CurrentRound;
                CurrentCombatParticipant = combat.CurrentParticipant;
                UpdateCampaign();
            }
        }

        [RelayCommand]
        private void NextTurn()
        {
            if (SelectedItem is Combat combat && combat.Participants.Any())
            {
                combat.CurrentParticipant.IsActive = false;
                
                if (combat.CurrentTurnIndex >= combat.Participants.Count - 1)
                {
                    combat.CurrentTurnIndex = 0;
                    combat.CurrentRound++;
                }
                else
                {
                    combat.CurrentTurnIndex++;
                }
                
                combat.CurrentParticipant = combat.Participants[combat.CurrentTurnIndex];
                combat.CurrentParticipant.IsActive = true;
                
                CurrentCombatRound = combat.CurrentRound;
                CurrentCombatParticipant = combat.CurrentParticipant;
                UpdateCampaign();
            }
        }

        [RelayCommand]
        private void EndCombat()
        {
            if (SelectedItem is Combat combat)
            {
                foreach (var participant in combat.Participants)
                {
                    participant.IsActive = false;
                }
                
                IsCombatActive = false;
                UpdateCampaign();
            }
        }

        private void SortCombatParticipants(Combat combat)
        {
            combat.Participants = new ObservableCollection<CombatParticipant>(
                combat.Participants.OrderByDescending(p => p.Initiative));
        }

        partial void OnCampaignChanged(Campaign oldValue, Campaign newValue)
        {
            if (oldValue != null)
            {
                oldValue.PlotItems.CollectionChanged -= (s, e) => col.Update(oldValue);
            }
            SubscribeToCampaignChanges();
        }

        partial void OnSelectedItemChanged(object newValue)
        {
            if (newValue != null)
            {
                Application.Current.Properties["CurrentCampaign"] = Campaign;
                
                if (newValue is Combat combat)
                {
                    Application.Current.Properties["SelectedPlotItem"] = combat;
                    SelectedTab = tabs[1];
                    UpdateCombatParticipants(combat);
                    
                    IsCombatActive = combat.CurrentParticipant != null;
                    if (IsCombatActive)
                    {
                        CurrentCombatRound = combat.CurrentRound;
                        CurrentCombatParticipant = combat.CurrentParticipant;
                    }
                }
                else if (newValue is StoryElement story)
                {
                    Application.Current.Properties["SelectedPlotItem"] = story;
                    SelectedTab = tabs[0];
                    CombatParticipants.Clear();
                    IsCombatActive = false;
                }
            }
        }
        
        [RelayCommand]
        private void AddPlayerToCombat(PlayerCharacter player)
        {
            if (SelectedItem is Combat combat && player != null)
            {
                // Проверка на дублирование
                if (combat.Participants.Any(p => p.SourceId.Equals(player.Id) && p.Type == ParticipantType.Player))
                {
                    MessageBox.Show("Этот персонаж уже добавлен в бой");
                    return;
                }

                var participant = new CombatParticipant
                {
                    Name = player.Name,
                    Initiative = player.Initiative,
                    CurrentHitPoints = player.CurrentHitPoints,
                    MaxHitPoints = player.MaxHitPoints,
                    ArmorClass = player.ArmorClass,
                    SourceId = player.Id,
                    Type = ParticipantType.Player
                };
        
                combat.Participants.Add(participant);
                SortCombatParticipants(combat);
                UpdateCampaign();
                MessageBox.Show($"{player.Name} добавлен в бой");
            }
        }
        
        [RelayCommand]
        private void AddNPCToCombat(NPC npc)
        {
            if (SelectedItem is Combat combat && npc != null)
            {
                // Проверка на дублирование
                if (combat.Participants.Any(p => p.SourceId.Equals(npc.Id) && p.Type == ParticipantType.Npc))
                {
                    MessageBox.Show("Этот NPC уже добавлен в бой");
                    return;
                }

                var participant = new CombatParticipant
                {
                    Name = npc.Name,
                    Initiative = new Random().Next(1, 20),
                    CurrentHitPoints = 10, // Можно добавить HP для NPC в классе NPC
                    MaxHitPoints = 10,
                    ArmorClass = 10,
                    SourceId = npc.Id,
                    Type = ParticipantType.Npc
                };
        
                combat.Participants.Add(participant);
                SortCombatParticipants(combat);
                UpdateCampaign();
                MessageBox.Show($"{npc.Name} добавлен в бой");
            }
        }
        
        [RelayCommand]
        private void AddEnemyToCombat(Enemy enemy)
        {
            if (SelectedItem is Combat combat && enemy != null)
            {
                // Проверка на дублирование
                if (combat.Participants.Any(p => p.SourceId.Equals(enemy.Id) && p.Type == ParticipantType.Enemy))
                {
                    MessageBox.Show("Этот враг уже добавлен в бой");
                    return;
                }

                var participant = new CombatParticipant
                {
                    Name = enemy.Name,
                    Initiative = new Random().Next(1, 20) + (enemy.Dexterity ?? 10 - 10) / 2,
                    CurrentHitPoints = enemy.Hit_points ?? 0,
                    MaxHitPoints = enemy.Hit_points ?? 0,
                    ArmorClass = enemy.Armor_class ?? 10,
                    SourceId = enemy.Id,
                    Type = ParticipantType.Enemy
                };
        
                combat.Participants.Add(participant);
                SortCombatParticipants(combat);
                UpdateCampaign();
                MessageBox.Show($"{enemy.Name} добавлен в бой");
            }
        }

        private void UpdateCombatParticipants(Combat combat)
        {
            CombatParticipants = new ObservableCollection<CombatParticipant>(combat.Participants);
        }
    }
}