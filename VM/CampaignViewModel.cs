using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DnDPartyManager.VM
{
    public partial class CampaignViewModel : ObservableObject
    {
        private static readonly ILiteCollection<Campaign> col = DBHelper.DB.GetCollection<Campaign>("campaigns");
        private static readonly ILiteCollection<Character> characterCol = DBHelper.DB.GetCollection<Character>("characters");
        private static readonly ILiteCollection<Character> npcCol = DBHelper.DB.GetCollection<Character>("npcs");
        private static readonly ILiteCollection<Enemy> enemyCol = DBHelper.DB.GetCollection<Enemy>("enemies");
        private readonly SignalRService _signalRService;

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
        private ObservableCollection<object> combatParticipants;

        [ObservableProperty]
        private string currentActionMessage;

        public CampaignViewModel()
        {
            _signalRService = new SignalRService();
            tabs =
            [
                new Tab() { Name = "Сюжет", Uri = new Uri("/V/UserControls/StorySettings.xaml", UriKind.Relative) },
                new Tab() { Name = "Бой", Uri = new Uri("/V/UserControls/CombatSettings.xaml", UriKind.Relative) }
            ];

            Load();

            // Подключение к SignalR
            _signalRService.ConnectAsync().ContinueWith(async _ =>
            {
                await _signalRService.JoinCombatAsync("combat1", true); // Мастер присоединяется к бою
            });

            // Обработка действий игрока
            _signalRService.OnPlayerActionReceived += (playerId, action, targetId, diceRoll) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentActionMessage = $"Игрок {playerId} выполнил действие: {action} на цель {targetId} с броском {diceRoll}";
                    var result = MessageBox.Show(CurrentActionMessage + "\nПодтвердить действие?", "Подтверждение", MessageBoxButton.YesNo);
                    int damage = diceRoll >= 10 ? 5 : 0; // Пример расчёта урона
                    _signalRService.ConfirmActionAsync("combat1", playerId, action, targetId, result == MessageBoxResult.Yes, damage);
                });
            };
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
            MigrateCombatData();
            SubscribeToCampaignChanges();
            UpdateCombatParticipants();
        }

        private void MigrateCombatData()
        {
            foreach (var campaign in Campaigns)
            {
                foreach (var item in campaign.PlotItems.OfType<Combat>())
                {
                    if (item.Participants == null)
                    {
                        item.Participants = new Dictionary<int, Character>();
                    }
                    if (item.Enemies == null)
                    {
                        item.Enemies = new Dictionary<int, Enemy>();
                    }
                }
            }
            col.Update(Campaigns);
        }

        private void SubscribeToCampaignChanges()
        {
            Campaign.PlotItems.CollectionChanged += (s, e) =>
            {
                col.Update(Campaign);
                UpdateCombatParticipants();
            };
            foreach (var item in Campaign.PlotItems)
            {
                if (item is Combat combat)
                {
                    SubscribeToCombatChanges(combat);
                }
                else if (item is StoryElement story)
                {
                    SubscribeToStoryChanges(story);
                }
            }
        }

        private void SubscribeToCombatChanges(Combat combat)
        {
            combat.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Combat.CurrentTurn) || e.PropertyName == nameof(Combat.TurnCounter) || e.PropertyName == nameof(Combat.Participants) || e.PropertyName == nameof(Combat.Enemies))
                {
                    UpdateCombatParticipants();
                }
                col.Update(Campaign);
            };
            foreach (var participant in combat.Participants.Values)
            {
                participant.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(Character.Hp) || e.PropertyName == nameof(Character.Name) || e.PropertyName == nameof(Character.Initiative))
                    {
                        if (participant is PlayerCharacter pc)
                            characterCol.Update(participant);
                        else if (participant is NPC npc)
                            npcCol.Update(participant);
                        if (e.PropertyName == nameof(Character.Initiative))
                        {
                            var oldInitiative = combat.Participants.FirstOrDefault(x => x.Value == participant).Key;
                            if (oldInitiative != participant.Initiative)
                            {
                                combat.Participants.Remove(oldInitiative);
                                combat.Participants[participant.Initiative] = participant;
                            }
                        }
                        col.Update(Campaign);
                        UpdateCombatParticipants(combat);
                    }
                };
            }
            foreach (var enemy in combat.Enemies.Values)
            {
                enemy.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(Enemy.Hit_points) || e.PropertyName == nameof(Enemy.Name) || e.PropertyName == nameof(Enemy.Initiative))
                    {
                        enemyCol.Update(enemy);
                        if (e.PropertyName == nameof(Enemy.Initiative))
                        {
                            var oldInitiative = combat.Enemies.FirstOrDefault(x => x.Value == enemy).Key;
                            if (oldInitiative != enemy.Initiative)
                            {
                                combat.Enemies.Remove(oldInitiative);
                                combat.Enemies[enemy.Initiative] = enemy;
                            }
                        }
                        col.Update(Campaign);
                        UpdateCombatParticipants(combat);
                    }
                };
            }
        }

        private void SubscribeToStoryChanges(StoryElement story)
        {
            story.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(StoryElement.Name) || e.PropertyName == nameof(StoryElement.Description))
                {
                    col.Update(Campaign);
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
            var storyElement = new StoryElement() { Name = "Новый сюжет", Description = "Описание сюжета" };
            Campaign.PlotItems.Add(storyElement);
            Console.WriteLine("Add story " + storyElement.Name);
            SubscribeToStoryChanges(storyElement);
            UpdateCampaign();
        }

        [RelayCommand]
        private void AddCombat()
        {
            var combat = new Combat() { Name = "Новый бой" };
            Campaign.PlotItems.Add(combat);
            Console.WriteLine("Add combat " + combat.Name);
            SubscribeToCombatChanges(combat);
            UpdateCampaign();
        }

        [RelayCommand]
        private void DeleteItem(object item)
        {
            if (SelectedItem is Combat combat)
            {
                if (item is Character character)
                {
                    var initiative = combat.Participants.FirstOrDefault(x => x.Value == character).Key;
                    if (combat.Participants.ContainsKey(initiative))
                    {
                        combat.Participants.Remove(initiative);
                        Console.WriteLine($"Removed character from combat: {character.Name}");
                    }
                }
                else if (item is Enemy enemy)
                {
                    var initiative = combat.Enemies.FirstOrDefault(x => x.Value == enemy).Key;
                    if (combat.Enemies.ContainsKey(initiative))
                    {
                        combat.Enemies.Remove(initiative);
                        Console.WriteLine($"Removed enemy from combat: {enemy.Name}");
                    }
                }
                col.Update(Campaign);
                UpdateCombatParticipants();
            }
            else if (Campaign.PlotItems.Contains(item))
            {
                Campaign.PlotItems.Remove(item);
                Console.WriteLine("Deleted plot item: " + item.ToString());
                UpdateCampaign();
            }
        }

        [RelayCommand]
        private void NextTurn()
        {
            if (SelectedItem is Combat combat)
            {
                var allInitiatives = combat.Participants.Keys.Concat(combat.Enemies.Keys)
                    .OrderByDescending(x => x).ToList();
                if (allInitiatives.Any())
                {
                    int currentIndex = allInitiatives.IndexOf(combat.CurrentTurn);
                    if (currentIndex < allInitiatives.Count - 1)
                    {
                        combat.CurrentTurn = allInitiatives[currentIndex + 1];
                    }
                    else
                    {
                        combat.CurrentTurn = allInitiatives[0];
                        combat.TurnCounter++;
                    }
                    col.Update(Campaign);
                    UpdateCombatParticipants();
                    _signalRService.ChangeTurnAsync("combat1", combat.CurrentTurn);
                }
            }
        }

        [RelayCommand]
        private void FinishCombat()
        {
            if (SelectedItem is Combat combat)
            {
                combat.IsFinished = true;
                col.Update(Campaign);
                UpdateCombatParticipants();
            }
        }

        [RelayCommand]
        private async Task StartCombat()
        {
            if (SelectedItem is Combat combat)
            {
                await _signalRService.StartCombatAsync("combat1");
            }
        }

        partial void OnCampaignChanged(Campaign oldValue, Campaign newValue)
        {
            Console.WriteLine(newValue.Name);
            if (oldValue != null)
            {
                oldValue.PlotItems.CollectionChanged -= (s, e) => col.Update(oldValue);
                foreach (var item in oldValue.PlotItems.OfType<Combat>())
                {
                    item.PropertyChanged -= (s, e) => col.Update(oldValue);
                }
                foreach (var item in oldValue.PlotItems.OfType<StoryElement>())
                {
                    item.PropertyChanged -= (s, e) => col.Update(oldValue);
                }
            }
            SubscribeToCampaignChanges();
            UpdateCombatParticipants();
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
                    UpdateCombatParticipants(combat);
                }
                else if (newValue is StoryElement story)
                {
                    Console.WriteLine($"Chose StoryElement: {story.Name}");
                    Application.Current.Properties["SelectedPlotItem"] = story;
                    SelectedTab = tabs[0];
                    CombatParticipants = new ObservableCollection<object>();
                }
            }
        }

        private void UpdateCombatParticipants(Combat combat = null)
        {
            combat ??= SelectedItem as Combat;
            if (combat != null)
            {
                var participants = new ObservableCollection<object>();
                foreach (var pair in combat.Participants.OrderByDescending(x => x.Key))
                {
                    var character = pair.Value;
                    participants.Add(character);
                    character.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(Character.Hp) || e.PropertyName == nameof(Character.Name) || e.PropertyName == nameof(Character.Initiative))
                        {
                            if (character is PlayerCharacter pc)
                                characterCol.Update(character);
                            else if (character is NPC npc)
                                npcCol.Update(character);
                            if (e.PropertyName == nameof(Character.Initiative))
                            {
                                var oldInitiative = combat.Participants.FirstOrDefault(x => x.Value == character).Key;
                                if (oldInitiative != character.Initiative)
                                {
                                    combat.Participants.Remove(oldInitiative);
                                    combat.Participants[character.Initiative] = character;
                                }
                            }
                            col.Update(Campaign);
                            UpdateCombatParticipants(combat);
                        }
                    };
                }
                foreach (var pair in combat.Enemies.OrderByDescending(x => x.Key))
                {
                    var enemy = pair.Value;
                    participants.Add(enemy);
                    enemy.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(Enemy.Hit_points) || e.PropertyName == nameof(Enemy.Name) || e.PropertyName == nameof(Enemy.Initiative))
                        {
                            enemyCol.Update(enemy);
                            if (e.PropertyName == nameof(Enemy.Initiative))
                            {
                                var oldInitiative = combat.Enemies.FirstOrDefault(x => x.Value == enemy).Key;
                                if (oldInitiative != enemy.Initiative)
                                {
                                    combat.Enemies.Remove(oldInitiative);
                                    combat.Enemies[enemy.Initiative] = enemy;
                                }
                            }
                            col.Update(Campaign);
                            UpdateCombatParticipants(combat);
                        }
                    };
                }
                CombatParticipants = participants;
                if (combat.CurrentTurn == 0 && participants.Any())
                {
                    combat.CurrentTurn = participants.First() is Character ch ? ch.Initiative : (participants.First() as Enemy).Initiative;
                }
            }
            else
            {
                CombatParticipants = new ObservableCollection<object>();
            }
        }
    }
}