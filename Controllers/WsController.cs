using Microsoft.AspNetCore.Mvc;

namespace WSServer.Controllers;

public class WSController : ControllerBase
{
    private readonly IConnectionManager connectionManager;

    public WSController(IConnectionManager connectionManager) 
    {
        this.connectionManager = connectionManager;
    }
    
    [Route("/ws")]
    public async Task Connection()
    {        
       if (HttpContext.WebSockets.IsWebSocketRequest)
       {
           using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
           var socketFinishedTcs = new TaskCompletionSource<object>();
           this.connectionManager.AddConnection(webSocket, socketFinishedTcs);
           await socketFinishedTcs.Task;
       }
       else
       {
           HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
       }
    }
}