using MariamApp.Hub;
using Microsoft.AspNetCore.SignalR;

namespace MariamApp.Helpers;

public class SignalRService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendReaportReadyMessageToUser(string userId, string message)
    {
        var hubConnectionId = NotificationHub.GetConnectionId(userId);

        if (hubConnectionId is not null)
        {
            await _hubContext.Clients.Client(hubConnectionId)
                .SendAsync("ReceiveReportNotification", message);
        }
    }
    
    public async Task SendStockUpdateMessageToUser(        
        string transactionType, 
        int transactionQuantity,
        string productName, 
        int stockQuantity)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveStockUpdate", 
            $"{transactionType} {transactionQuantity} item",
            $"{productName}",
            $"{stockQuantity}");
    }
}