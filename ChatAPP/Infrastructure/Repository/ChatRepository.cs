using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAPP.Database;
using ChatAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPP.Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private AppDbContext _ctx;

        public ChatRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<Message> CreateMessage(int chatId, string message, string userId)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = userId,
                Timestamp = DateTime.Now
            };

            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();

            return Message;
        }

        public async Task<int> CreatePrivateRoom(string roomId, string targetId)
        {
            var chat = new Chat
            {
                Type = ChatType.Private
            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = roomId
            });
            chat.Name = chat.Id.ToString();
            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            return chat.Id;
        }
        public Chat FindChats(string userId1, string userId2)
        {
            var user1 = GetUserById(userId1);
            var user2 = GetUserById(userId2);
            var chats = _ctx.Chats.Select(x => x).Where(x => x.Users.FirstOrDefault(x=>x.UserId==userId1)!=null &&
            x.Users.FirstOrDefault(x=>x.UserId==userId2)!=null).ToList();
            var chat = chats.FirstOrDefault(x=>x.Type==ChatType.Private);
            return chat;
        }
        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });
            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();
        }
        public async Task RemoveRoom(string name,string userId)
        {
            var chat = GetChatByName(name);
            var user = chat.Users.FirstOrDefault(x => x.UserId == userId);
            if (user.Role == UserRole.Admin)
            {
                _ctx.Chats.Remove(GetChatByName(name));
            }
            await _ctx.SaveChangesAsync();
        }
        public ChatUser GetUserById(string id)
        {
            return _ctx.ChatUsers.FirstOrDefault(x => x.User.Id == id);
        }
        public string GetUserName(string id)
        {
            return _ctx.Users.FirstOrDefault(x => x.Id == id).UserName;
        }
        public Chat GetChat(int id)
        {
            return _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }
        public Chat GetChatByName(string name)
        {
            return _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Name == name);
        }
        public IEnumerable<Chat> GetChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => x.Type == ChatType.Room && !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }
        public IEnumerable<Chat> GetPublicChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => x.Type == ChatType.Room && x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }
        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _ctx.Chats
                   .Include(x => x.Users)
                       .ThenInclude(x => x.User)
                   .Where(x => x.Type == ChatType.Private
                       && x.Users
                           .Any(y => y.UserId == userId))
                   .ToList();
        }
        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _ctx.ChatUsers.Add(chatUser);

            await _ctx.SaveChangesAsync();
        }
    }
}