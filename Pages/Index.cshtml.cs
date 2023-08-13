using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WSServer.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    private readonly IConnectionManager wsConnectionManager;

    [BindProperty]
    public string Status {get; set;}

    public IndexModel(ILogger<IndexModel> logger, IConnectionManager wsConnectionManager)
    {        
        this.logger = logger;
        this.wsConnectionManager = wsConnectionManager;
        this.Status = "";
    }

    public void OnPost(string status)
    {
        ViewData["currentStatus"] = status;
        this.wsConnectionManager.Broadcast(status);
    }
}
