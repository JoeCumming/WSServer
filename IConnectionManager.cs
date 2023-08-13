public interface IConnectionManager {

    void AddConnection(System.Net.WebSockets.WebSocket ws, TaskCompletionSource<object> taskCompletion);    
    void Broadcast(string status);
}