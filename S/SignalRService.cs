using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DnDPartyManager.S
{
    public class SignalRService
    {
        private readonly HubConnection _connection;

        public event Action<string, string, int, int> OnPlayerActionReceived;
        public event Action OnCombatStarted;
        public event Action<int> OnTurnChanged;

        public SignalRService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/combatHub") // Укажите URL вашего SignalR-сервера
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string, string, int, int>("ReceivePlayerAction", (playerId, action, targetId, diceRoll) =>
            {
                OnPlayerActionReceived?.Invoke(playerId, action, targetId, diceRoll);
            });

            _connection.On("CombatStarted", () =>
            {
                OnCombatStarted?.Invoke();
            });

            _connection.On<int>("TurnChanged", (currentTurn) =>
            {
                OnTurnChanged?.Invoke(currentTurn);
            });
        }

        public async Task ConnectAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }
        }

        public async Task JoinCombatAsync(string combatId, bool isMaster = false)
        {
            await _connection.InvokeAsync("JoinCombat", combatId, isMaster);
        }

        public async Task StartCombatAsync(string combatId)
        {
            await _connection.InvokeAsync("StartCombat", combatId);
        }

        public async Task ChangeTurnAsync(string combatId, int currentTurn)
        {
            await _connection.InvokeAsync("ChangeTurn", combatId, currentTurn);
        }

        public async Task ConfirmActionAsync(string combatId, string playerId, string action, int targetId, bool isApproved, int damage)
        {
            await _connection.InvokeAsync("ConfirmAction", combatId, playerId, action, targetId, isApproved, damage);
        }
    }
}