using System.Text.Json;
using Fleck;
using infastructure.Repositories;
using lib;
using service;

namespace api.ClientWants;

public class ClientWantsToEnterRoomDto : BaseDto
{
    public int roomId { get; set; }
}

public class ClientWantsToEnterRoom(MessageService messageService) : BaseEventHandler<ClientWantsToEnterRoomDto>
{
    public override Task Handle(ClientWantsToEnterRoomDto dto, IWebSocketConnection socket)
    {
        var isSuccess = WebSocketStateService.AddToRoom(socket, dto.roomId);
        socket.Send(JsonSerializer.Serialize(new ServerAddsClientToRoom()
        {
            message = "you were succesfully added to room with ID " + dto.roomId,
            messages =         messageService.GetMassageFeed(dto.roomId)

        }));
        return Task.CompletedTask;
    }
}
public class ServerAddsClientToRoom : BaseDto
{
    public IEnumerable<MessageRepository.MessagesFeedQuery> messages { get; set; }
    public String message { get; set; }
}