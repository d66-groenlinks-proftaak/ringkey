using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ringkey.Common.Models.Messages;
using ringkey.Logic;

namespace ringkey.Controllers
{
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }
        
        [HttpPost("/message/new")]
        public async Task<IActionResult> CreateMessage([FromBody] NewMessage message)
        {
            _messageService.CreateMessage(message);
            
            return new OkResult();
        } 
    }
}