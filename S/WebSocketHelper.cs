using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using DnDPartyManager.M;

public static class WebSocketHelper
{
    private static HttpListener _listener;
    private static readonly ObservableCollection<Client> _clients = new ObservableCollection<Client>();
    private static CancellationTokenSource _cts;
    
    public static int Port { get; private set; }
    public static bool IsRunning { get; private set; }
    public static ReadOnlyObservableCollection<Client> Clients { get; } 
        = new ReadOnlyObservableCollection<Client>(_clients);

    public static event Action<string> LogMessage;
    public static event Action<Client> ClientConnected;
    public static event Action<Client> ClientDisconnected;
    public static event Action<Client, string> MessageReceived;

    public static async Task Start(int port)
    {
        if (IsRunning) return;
        
        Port = port;
        _cts = new CancellationTokenSource();
        
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{port}/");
        _listener.Start();
        
        IsRunning = true;
        Log("Server started on port " + port);

        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var context = await _listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    ProcessRequest(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Log($"Server error: {ex.Message}");
        }
        finally
        {
            await Stop();
        }
    }

    private static async void ProcessRequest(HttpListenerContext context)
    {
        var ws = await context.AcceptWebSocketAsync(null);
        var socket = ws.WebSocket;
        
        var client = new Client
        {
            Name = $"Client_{Guid.NewGuid().ToString()[..8]}",
            Ip = context.Request.RemoteEndPoint?.Address.ToString() ?? "unknown"
        };
        
        Application.Current.Dispatcher.Invoke(() =>
        {
            _clients.Add(client);
        });
        
        Log($"New client connected: {client.Name} ({client.Ip})");
        ClientConnected?.Invoke(client);
        
        try
        {
            var buffer = new byte[1024];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), _cts.Token);
                
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Log($"Received from {client.Name}: {message}");
                    MessageReceived?.Invoke(client, message);
                }
            }
        }
        catch (Exception ex)
        {
            Log($"Client {client.Name} error: {ex.Message}");
        }
        finally
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _clients.Remove(client);
            });
            
            socket.Dispose();
            Log($"Client disconnected: {client.Name}");
            ClientDisconnected?.Invoke(client);
        }
    }

    public static async Task Stop()
    {
        if (!IsRunning) return;
        
        _cts?.Cancel();
        
        // Очищаем список клиентов
        Application.Current.Dispatcher.Invoke(() =>
        {
            _clients.Clear();
        });
        
        _listener?.Stop();
        IsRunning = false;
        Log("Server stopped");
    }

    public static async Task Broadcast(string message)
    {
        if (!IsRunning) return;
        
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);
        
        // Здесь можно добавить реальную рассылку через WebSocket
        // если хранить соединения в отдельном словаре
        
        Log($"Broadcast message: {message}");
    }

    public static async Task SendToClient(Client client, string message)
    {
        // Реализация отправки конкретному клиенту
        // Требует хранения связи между Client и WebSocket
        Log($"Sent to {client.Name}: {message}");
    }

    private static void Log(string message)
    {
        Debug.WriteLine($"[WsServer] {message}");
        Application.Current.Dispatcher.Invoke(() => 
            LogMessage?.Invoke(message));
    }
}