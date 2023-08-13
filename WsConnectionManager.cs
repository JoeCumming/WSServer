using System.Net.WebSockets;
using System.Text;

namespace Controllers;

public class WsConnectionManager : IConnectionManager {

    private readonly ILogger<WsConnectionManager> _logger;

    private Dictionary<WebSocket, TaskCompletionSource<object>> connections = new Dictionary<WebSocket, TaskCompletionSource<object>>();

    public WsConnectionManager(ILogger<WsConnectionManager> logger) 
    {
        this._logger = logger;
    }

    public void AddConnection(WebSocket ws, TaskCompletionSource<object> taskCompletion) 
    {
        this.connections.Add(ws, taskCompletion);  
        this.AwaitDisconnect(ws, taskCompletion);        
    }

    private async void AwaitDisconnect(WebSocket ws,TaskCompletionSource<object> taskCompletion) 
    {   
        try 
        {
            // Treat any response from the client as a disconnect. 
            // TODO: refactor to a heartbeat
            await ws.ReceiveAsync(Array.Empty<byte>(), CancellationToken.None); 
        }   
        catch (WebSocketException e) {
            Console.WriteLine(e);
        }
        finally {
            taskCompletion.SetResult(true);
            connections.Remove(ws);
        }          
    }

    public void Broadcast(string status) 
    {
        var data = Encoding.ASCII.GetBytes(status);
        connections.Keys.ToList().ForEach(ws => ws.SendAsync(data, WebSocketMessageType.Text,  true, CancellationToken.None));
    }
}