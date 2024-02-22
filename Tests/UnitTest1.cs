using api.ClientWants;
using lib;
using Newtonsoft.Json;
using Websocket.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        Startup.Statup(null);
    }

    [Test]
    public async Task Test1()
    {
        var ws = await new WebSocketTestClient().ConnectAsync();
        var ws2 = await new WebSocketTestClient().ConnectAsync();
        await ws.DoAndAssert(new ClientWantsToSignInDto()
        {
            Username = "Alexander"
        }, result => result.Count(dto => dto.eventType == nameof(ServerWelcomesUser)) == 1 );
        
        await ws2.DoAndAssert(new ClientWantsToSignInDto()
        {
            Username = "Jesper"
        }, result => result.Count(dto => dto.eventType == nameof(ServerWelcomesUser)) == 1 );
        
        await ws.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 1
        }, result => result.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1 );
        
        await ws2.DoAndAssert(new ClientWantsToEnterRoomDto()
        {
            roomId = 1
        }, result => result.Count(dto => dto.eventType == nameof(ServerAddsClientToRoom)) == 1 );
        
        await ws.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
        {
            roomId = 1,
            message = "Hey Jesper"
        }, result => result.Count(dto => dto.eventType == nameof(ServerBroadcastsMessageWithUsername)) == 1 );
        
        await ws2.DoAndAssert(new ClientWantsToBroadcastToRoomDto()
        {
            roomId = 1,
            message = "Hey Alexander"
        }, result => result.Count(dto => dto.eventType == nameof(ServerBroadcastsMessageWithUsername)) == 2 );
    }
}