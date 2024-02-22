using System.Text.Json;
using Fleck;
using lib;
using service;

namespace api.ClientWants;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    public string message { get; set; }
    public int roomId { get; set; }
}

public class ClientWantsToBroadcastToRoom(MessageService messageService) : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    public override Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        var message = new ServerBroadcastsMessageWithUsername()
        {
            message = dto.message,
            username = WebSocketStateService.Connections[socket.ConnectionInfo.Id].Username
            
            
        };
        WebSocketStateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(
            message));
        return Task.CompletedTask;
    }
}

public class ServerBroadcastsMessageWithUsername : BaseDto
{
    public string message { get; set; }
    public string username { get; set; }
}