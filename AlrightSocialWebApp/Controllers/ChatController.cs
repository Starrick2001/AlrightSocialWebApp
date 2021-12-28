using AlrightSocialWebApp.Hubs;
using AlrightSocialWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlrightSocialWebApp.Controllers
{
    [Route("[controller]")]
    public class ChatController : Controller
    {
        DataContext _context = new DataContext();
        private IHubContext<ChatHub> _chat;
        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }
        public IActionResult ChatPageGUI()
        {
            return View();
        }
        [HttpGet("[action]")]
        public IActionResult Chat(int ChatId)
        {
            var chat = _context.Chats.Include(x => x.Messages).FirstOrDefault(x => x.Id == ChatId);
            if (chat.User1 != HttpContext.Session.GetString("email") && chat.User2 != HttpContext.Session.GetString("email"))
            {
                return RedirectToAction("Index", "HomePage");
            }

            return View(chat);
        }
        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom (string connectionId, string roomName)
        {
            _chat.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }
        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int chatId,
            string message,
            [FromServices] IHubContext<ChatHub> chat)
        {

            var Message = new Message
            {
                ChatId = chatId,
                Content = message,
                SenderEmail = HttpContext.Session.GetString("email"),
                Time = DateTime.Now
            };
            _context.Message.Add(Message);
            await _context.SaveChangesAsync();
            await chat.Clients.Group(chatId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Content = Message.Content,
                    SenderEmail = Message.SenderEmail,
                    Time = Message.Time.ToString("dd/MM/yyyy hh:mm:ss"),
                    ChatId = Message.ChatId
                });

            return Ok();
        }
        //[HttpPost]
        //public async Task<IActionResult> SendMessage(int ChatId, string message)
        //{
        //    var Message = new Message
        //    {
        //        ChatId = ChatId,
        //        Content = message,
        //        SenderEmail = HttpContext.Session.GetString("email"),
        //        Time = DateTime.Now
        //    };
        //    _context.Message.Add(Message);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Chat", new { ChatId = ChatId });
        //}
    }
}
