using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatAPP.Database;
using ChatAPP.Hubs;
using ChatAPP.Infrastructure;
using ChatAPP.Infrastructure.Repository;
using ChatAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAPP.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IChatRepository _repo;
        public HomeController(IChatRepository repo) => _repo = repo;

        public IActionResult Index()
        {
            var chats = _repo.GetChats(GetUserId());

            return View(chats);
        }
        [HttpGet]
        public IActionResult Find([FromServices] AppDbContext ctx)
        {
            var users = ctx.Users
                .Where(x => x.Id != User.GetUserId())
                .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            var chats = _repo.GetPrivateChats(GetUserId());

            return View(chats);
        }

        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            if (_repo.GetPrivateChats(userId).Contains(_repo.FindChats(userId, GetUserId())))
            {
                var chat = _repo.FindChats(userId, GetUserId());
                chat.Name = _repo.GetUserName(userId);
                return RedirectToAction("Chat",new { chat.Id });
            }
            var id = await _repo.CreatePrivateRoom(GetUserId(), userId);
            return RedirectToAction("Chat", new { id });
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            return View(_repo.GetChat(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _repo.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRoom(string name)
        {
            var chat = _repo.GetPublicChats(GetUserId());
            if (_repo.GetPublicChats(GetUserId()).Contains(_repo.GetChatByName(name)))
            {
                await _repo.RemoveRoom(name, GetUserId());
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _repo.JoinRoom(id, GetUserId());

            return RedirectToAction("Chat", "Home", new { id = id });
        }

        public async Task<IActionResult> SendMessage(
            int roomId,
            string message,
            [FromServices] IHubContext<ChatHub> chat)
        {
            var Message = await _repo.CreateMessage(roomId, message, User.Identity.Name);

            await chat.Clients.Group(roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }
    }
}