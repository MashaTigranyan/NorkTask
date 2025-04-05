using System.Security.Claims;
namespace MariamApp.Hub;


public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
{
    private static readonly Dictionary<string, string> _connections = new();
    
    public async override Task OnConnectedAsync()
    {
        var userId = Context.User?.Identity?.Name;        
        if (!string.IsNullOrEmpty(userId))
        {
            _connections[userId] = Context.ConnectionId!;
        }

        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User?.Identity?.Name;        

        if (!string.IsNullOrEmpty(userId))
        {
            _connections.Remove(userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public static string? GetConnectionId(string userId)
    {
        _connections.TryGetValue(userId, out var connectionId);
        return connectionId;
    }
}
