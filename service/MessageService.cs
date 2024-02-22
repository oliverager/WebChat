using infastructure.DataModels;
using infastructure.Repositories;

namespace service;

public class MessageService
{
    private readonly MessageRepository _messageRepository;

    public MessageService(MessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public IEnumerable<MessagesFeedQuery> GetMassageFeed()
    {
        return _messageRepository.GetMassageFeed();
    }

    public Message StoreMessage(string Messages, string Username, int RoomId)
    {
        return _messageRepository.CreateMessage(Messages, Username, RoomId);
    }
}

