using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPP.Models;

namespace ChatAPP.Infrastructure.Repository
{
    public interface IChatRepository
    {
        Chat GetChatByName(string name);
        Chat GetChat(int id);
        Task CreateRoom(string name, string userId);
        Task RemoveRoom(string name,string userId);
        Task JoinRoom(int chatId, string userId);
        string GetUserName(string id);
        ChatUser GetUserById(string id);
        Chat FindChats(string userId1, string userId2);
        IEnumerable<Chat> GetChats(string userId);
        IEnumerable<Chat> GetPublicChats(string userId);
        Task<int> CreatePrivateRoom(string rootId, string targetId);
        IEnumerable<Chat> GetPrivateChats(string userId);

        Task<Message> CreateMessage(int chatId, string message, string userId);
    }
}