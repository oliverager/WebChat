using Dapper;
using infastructure.DataModels;
using Npgsql;

namespace infastructure.Repositories;

public class MessageRepository
{
    private NpgsqlDataSource _dataSource;

    public MessageRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public IEnumerable<MessagesFeedQuery> GetMessageFeed(int roomId)
    {
        string sql = $@"
        SELECT messageId as {nameof(MessagesFeedQuery.MessageId)},
        messages as {nameof(MessagesFeedQuery.Messages)},
        username as {nameof(MessagesFeedQuery.Username)},
        roomId as {nameof(MessagesFeedQuery.RoomId)}
        FROM webchat.messages
        WHERE roomId = @RoomId;
    ";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<MessagesFeedQuery>(sql, new { RoomId = roomId });
        }
    }
    

    public Message CreateMessage(string Messages, string Username, int RoomId)
    {
        string sql = @"
        INSERT INTO webchat.messages (messages, username, roomid) 
        VALUES (@Messages, @Username, @RoomId)
        RETURNING  messageid as MessageId,
                messages as Messages,
                username as Username,
                roomid as RoomId
    ";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Message>(sql, new { Messages, Username, RoomId });
        }
    }


    public class MessagesFeedQuery
    {
        public int MessageId { get; set; }
        public string Messages { get; set; }
        public string Username { get; set; }
        public int RoomId { get; set; }
    }
}