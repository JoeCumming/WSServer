var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<IConnectionManager, Controllers.WsConnectionManager>();
var app = builder.Build();

app.UseWebSockets();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();
