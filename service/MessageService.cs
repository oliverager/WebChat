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

    public IEnumerable<MessageRepository.MessagesFeedQuery> GetMassageFeed(int roomId)
    {
        
        var messageFeed = _messageRepository.GetMessageFeed(roomId);
         
        foreach (var message in messageFeed)
        {
            Console.WriteLine($"Message ID: {message.MessageId}");
            Console.WriteLine($"Message: {message.Messages}");
            Console.WriteLine($"Username: {message.Username}");
            Console.WriteLine($"Room ID: {message.RoomId}");
            Console.WriteLine();
        }

        return messageFeed;
    }

    public Message StoreMessage(string Messages, string Username, int RoomId)
    {
        try
        {
            // Data validation
            if (string.IsNullOrEmpty(Messages) || string.IsNullOrEmpty(Username) || RoomId <= 0)
            {
                throw new ArgumentException("Invalid message data");
            }

            // Call the repository method to create and store the message
            return _messageRepository.CreateMessage(Messages, Username, RoomId);
        }
        catch (Exception ex)
        {
            // Exception handling
            Console.WriteLine($"Error storing message: {ex.Message}");
            throw; // Re-throw the exception to propagate it to the caller
        }
    }
}

