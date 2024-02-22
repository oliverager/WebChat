using System.Text.Json;
using Fleck;
using lib;

namespace api.ClientWants;

public class ClientWantsToEchoServerDto : BaseDto
{
    public string messageContent { get; set; }
}

public class ClientWantsToEchoServer : BaseEventHandler<ClientWantsToEchoServerDto>
{
    public override Task Handle(ClientWantsToEchoServerDto dto, IWebSocketConnection socket)
    {
        var echo = new ServerEchosClient()
        {
            echoValue = "echo: " + dto.messageContent
        };
        var messageToClient = JsonSerializer.Serialize(echo);
        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}

public class ClientWantsTimeOfMessage : BaseEventHandler<ClientWantsToEchoServerDto>
{
    public override Task Handle(ClientWantsToEchoServerDto dto, IWebSocketConnection socket)
    {
        var time = new TimeOfMessage()
        {
            TimeValue = DateTime.Now.ToString("F"),
            messageContent = dto.messageContent
            
        };
        var messageToClient = JsonSerializer.Serialize(time);
        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}

public class TimeOfMessage : BaseDto
{
    public string TimeValue { get; set; }
    public string messageContent { get; set; }
}

public class ServerEchosClient : BaseDto
{
    public string echoValue { get; set; }
}