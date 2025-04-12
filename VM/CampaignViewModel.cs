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

        [ObservableProperty]
        private ObservableCollection<PlayerCharacter> combatPlayers;

        [ObservableProperty]
        private ObservableCollection<PlayerCharacter> combatNpcs;

        [ObservableProperty]
        private ObservableCollection<Enemy> combatEnemies;

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
            UpdateCombatLists();
        }

        private void SubscribeToCampaignChanges()
        {
            // Подписываемся на изменения PlotItems
            Campaign.PlotItems.CollectionChanged += (s, e) =>
            {
                col.Update(Campaign); // Сохраняем при добавлении/удалении элементов
                UpdateCombatLists();
            };

            // Подписываемся на каждый Combat в PlotItems
            foreach (var item in Campaign.PlotItems.OfType<Combat>())
            {
                SubscribeToCombatChanges(item);
            }
        }

        private void SubscribeToCombatChanges(Combat combat)
        {
            // Подписка на изменение свойств Combat
            combat.PropertyChanged += (s, e) =>
            {
                col.Update(Campaign); // Обновляем Campaign при изменении Combat
            };

            // Подписка на изменения коллекций внутри Combat
            combat.PlayersIds.CollectionChanged += (s, e) => col.Update(Campaign);
            combat.NpcsIds.CollectionChanged += (s, e) => col.Update(Campaign);
            combat.Enemies.CollectionChanged += (s, e) =>
            {
                col.Update(Campaign);
                // Обновляем базу для новых врагов
                if (e.NewItems != null)
                {
                    foreach (Enemy enemy in e.NewItems)
                    {
                        enemyCol.Upsert(enemy);
                    }
                }
            };

            // Подписка на изменения HP врагов
            foreach (var enemy in combat.Enemies)
            {
                enemy.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(Enemy.Hit_points))
                    {
                        enemyCol.Update(enemy);
                        col.Update(Campaign);
                    }
                };
            }
        }

        [RelayCommand]
        private void UpdateCampaign()
        {
            col.Update(Campaign);
        }

        [RelayCommand]
        private void AddStory()
        {
            StoryElement storyElement = new StoryElement() { Name = "123123123123123" };
            Campaign.PlotItems.Add(storyElement);
            Console.WriteLine("Add story " + storyElement.Name);
            UpdateCampaign();
        }

        [RelayCommand]
        private void AddCombat()
        {
            var combat = new Combat() { Name = "stub" };
            Campaign.PlotItems.Add(combat);
            Console.WriteLine("Add stub combat");
            SubscribeToCombatChanges(combat); // Подписываемся на новый Combat
            UpdateCampaign();
        }

        [RelayCommand]
        private void DeleteItem(object item)
        {
            if (item != null && Campaign.PlotItems.Contains(item))
            {
                Campaign.PlotItems.Remove(item);
                Console.WriteLine("Delete stub " + item.ToString());
                UpdateCampaign();
            }
        }

        partial void OnCampaignChanged(Campaign oldValue, Campaign newValue)
        {
            Console.WriteLine(newValue.Name);
            if (oldValue != null)
            {
                // Отписываемся от старой Campaign
                oldValue.PlotItems.CollectionChanged -= (s, e) => col.Update(oldValue);
                foreach (var item in oldValue.PlotItems.OfType<Combat>())
                {
                    item.PropertyChanged -= (s, e) => col.Update(oldValue);
                    item.PlayersIds.CollectionChanged -= (s, e) => col.Update(oldValue);
                    item.NpcsIds.CollectionChanged -= (s, e) => col.Update(oldValue);
                    item.Enemies.CollectionChanged -= (s, e) => col.Update(oldValue);
                }
            }
            SubscribeToCampaignChanges();
            UpdateCombatLists();
        }

        partial void OnSelectedItemChanged(object newValue)
        {
            if (newValue != null)
            {
                Console.WriteLine($"Chose: {newValue}");
                Application.Current.Properties["CurrentCampaign"] = Campaign;
                if (newValue is Combat combat)
                {
                    Console.WriteLine($"Chose Combat: {combat.Name}");
                    Application.Current.Properties["SelectedPlotItem"] = combat;
                    SelectedTab = tabs[1];
                    UpdateCombatLists(combat);
                }
                else if (newValue is StoryElement story)
                {
                    Console.WriteLine($"Chose StoryElement: {story.Name}");
                    Application.Current.Properties["SelectedPlotItem"] = story;
                    SelectedTab = tabs[0];
                    CombatPlayers = new ObservableCollection<PlayerCharacter>();
                    CombatNpcs = new ObservableCollection<PlayerCharacter>();
                    CombatEnemies = new ObservableCollection<Enemy>();
                }
            }
        }

        private void UpdateCombatLists(Combat combat = null)
        {
            combat ??= SelectedItem as Combat;
            if (combat != null)
            {
                CombatPlayers = new ObservableCollection<PlayerCharacter>(
                    combat.PlayersIds.Select(id => playerCol.FindById(id)).Where(p => p != null));
                CombatNpcs = new ObservableCollection<PlayerCharacter>(
                    combat.NpcsIds.Select(id => playerCol.FindById(id)).Where(p => p != null));
                CombatEnemies = combat.Enemies;

                // Подписываемся на изменения HP игроков и NPC
                foreach (var player in CombatPlayers)
                {
                    player.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(PlayerCharacter.CurrentHitPoints))
                        {
                            playerCol.Update(player);
                        }
                    };
                }
                foreach (var npc in CombatNpcs)
                {
                    npc.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(PlayerCharacter.CurrentHitPoints))
                        {
                            playerCol.Update(npc);
                        }
                    };
                }
            }
            else
            {
                CombatPlayers = new ObservableCollection<PlayerCharacter>();
                CombatNpcs = new ObservableCollection<PlayerCharacter>();
                CombatEnemies = new ObservableCollection<Enemy>();
            }
        }
    }
}