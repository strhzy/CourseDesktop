using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDPartyManager.M;
using DnDPartyManager.S;
using LiteDB;
using System.Linq;
using System.Threading.Tasks;

namespace DnDPartyManager.VM
{
    public partial class EnemiesViewModel : ObservableObject
    {
        private static readonly ILiteCollection<Campaign> campaignCol = DBHelper.DB.GetCollection<Campaign>("campaigns");
        private readonly ILiteCollection<Enemy> enemyCol = DBHelper.DB.GetCollection<Enemy>("enemies");

        [ObservableProperty]
        private List<string> slugs;

        [ObservableProperty]
        private Enemy selectedEnemy;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private ObservableCollection<Enemy> enemies = new ObservableCollection<Enemy>();

        [ObservableProperty]
        private ObservableCollection<Combat> combats;

        [ObservableProperty]
        private Combat selectedCombat;

        public EnemiesViewModel()
        {
            Application.Current.Dispatcher.InvokeAsync(async () => await LoadEnemies());
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
        private async Task LoadEnemies()
        {
            Enemies.Clear();
            var newEnemies = await DBHelper.GetEnemiesAsync(CurrentPage);
            foreach (var enemy in newEnemies)
            {
                Enemies.Add(enemy);
            }
        }

        [RelayCommand]
        private async Task NextPage()
        {
            CurrentPage++;
            await LoadEnemies();
        }

        [RelayCommand]
        private async Task PrevPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadEnemies();
            }
        }

        [RelayCommand]
        private void AddToCombat()
        {
            if (SelectedEnemy == null)
            {
                MessageBox.Show("Пожалуйста, выберите врага.");
                return;
            }

            if (SelectedCombat == null)
            {
                MessageBox.Show("Пожалуйста, выберите бой.");
                return;
            }

            if (SelectedCombat.Enemies.Values.Any(e => e.Name == SelectedEnemy.Name && e.Hit_points == SelectedEnemy.Hit_points))
            {
                MessageBox.Show("Этот враг уже добавлен в бой.");
                return;
            }

            // Клонируем врага
            var enemyClone = new Enemy
            {
                Id = System.Guid.NewGuid().ToString(),
                Slug = SelectedEnemy.Slug,
                Name = SelectedEnemy.Name,
                Desc = SelectedEnemy.Desc,
                Size = SelectedEnemy.Size,
                Type = SelectedEnemy.Type,
                Subtype = SelectedEnemy.Subtype,
                Group = SelectedEnemy.Group,
                Alignment = SelectedEnemy.Alignment,
                Armor_class = SelectedEnemy.Armor_class,
                Armor_desc = SelectedEnemy.Armor_desc,
                Hit_points = SelectedEnemy.Hit_points,
                Hit_dice = SelectedEnemy.Hit_dice,
                Speed = SelectedEnemy.Speed != null ? new Dictionary<string, object>(SelectedEnemy.Speed) : null,
                Strength = SelectedEnemy.Strength,
                Dexterity = SelectedEnemy.Dexterity,
                Constitution = SelectedEnemy.Constitution,
                Intelligence = SelectedEnemy.Intelligence,
                Wisdom = SelectedEnemy.Wisdom,
                Charisma = SelectedEnemy.Charisma,
                Perception = SelectedEnemy.Perception,
                Skills = SelectedEnemy.Skills != null ? new Dictionary<string, int>(SelectedEnemy.Skills) : null,
                Damage_vulnerabilities = SelectedEnemy.Damage_vulnerabilities,
                Damage_resistances = SelectedEnemy.Damage_resistances,
                Damage_immunities = SelectedEnemy.Damage_immunities,
                Condition_immunities = SelectedEnemy.Condition_immunities,
                Senses = SelectedEnemy.Senses,
                Languages = SelectedEnemy.Languages,
                Challenge_rating = SelectedEnemy.Challenge_rating,
                Cr = SelectedEnemy.Cr,
                Actions = SelectedEnemy.Actions != null ? new List<Attack>(SelectedEnemy.Actions) : new List<Attack>(),
                Bonus_actions = SelectedEnemy.Bonus_actions != null ? new List<Attack>(SelectedEnemy.Bonus_actions) : new List<Attack>(),
                Reactions = SelectedEnemy.Reactions != null ? new List<Attack>(SelectedEnemy.Reactions) : new List<Attack>(),
                Legendary_desc = SelectedEnemy.Legendary_desc,
                Legendary_actions = SelectedEnemy.Legendary_actions != null ? new List<Attack>(SelectedEnemy.Legendary_actions) : new List<Attack>(),
                Special_abilities = SelectedEnemy.Special_abilities != null ? new List<SpecialAbility>(SelectedEnemy.Special_abilities) : new List<SpecialAbility>(),
                Spell_list = SelectedEnemy.Spell_list != null ? new List<string>(SelectedEnemy.Spell_list) : new List<string>(),
                Initiative = SelectedEnemy.Initiative
            };

            // Проверяем уникальность инициативы
            int initiative = enemyClone.Initiative;
            while (SelectedCombat.Participants.ContainsKey(initiative) || SelectedCombat.Enemies.ContainsKey(initiative))
            {
                initiative++;
            }
            enemyClone.Initiative = initiative;

            // Добавляем врага в бой
            SelectedCombat.Enemies[enemyClone.Initiative] = enemyClone;
            enemyCol.Insert(enemyClone);

            // Обновляем кампанию в базе
            var campaign = campaignCol.FindAll().FirstOrDefault();
            if (campaign != null)
            {
                campaignCol.Update(campaign);
            }

            MessageBox.Show($"{enemyClone.Name} добавлен в бой {SelectedCombat.Name}.");
        }

        partial void OnSelectedEnemyChanged(Enemy newValue)
        {
            if (newValue != null)
            {
                Console.WriteLine($"Selected enemy: {newValue.Name}");
            }
        }
    }
}