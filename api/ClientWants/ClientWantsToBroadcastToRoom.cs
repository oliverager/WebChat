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

public class ClientWantsToBroadcastToRoom : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    private readonly MessageService _messageService;

    public ClientWantsToBroadcastToRoom(MessageService messageService)
    {
        _messageService = messageService;
    }

    public override Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        try
        {
            var message = new ServerBroadcastsMessageWithUsername()
            {
                message = dto.message,
                username = WebSocketStateService.Connections[socket.ConnectionInfo.Id].Username
            };

            _messageService.StoreMessage(dto.message, message.username, dto.roomId);
            
            WebSocketStateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(message));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client broadcast request: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}


public class ServerBroadcastsMessageWithUsername : BaseDto
{
    public string message { get; set; }
    public string username { get; set; }
}