using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;
using ringkey.Logic;
using ringkey.Logic.Accounts;

namespace ringkey.Controllers
{
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;
        private readonly AccountService _accountService;

        public MessageController(MessageService messageService, AccountService accountService)
        {
            _messageService = messageService;
            _accountService = accountService;
        }

        [HttpPost("message/create")]
        public async Task<ActionResult> Upload(NewMessage message)
        {
            Account acc = _accountService.GetByToken(message.Token);
            MessageErrors error;
            
            if(acc != null)
                error = _messageService.CreateMessage(message, acc);
            else
                error = _messageService.CreateMessage(message, null);

            Console.WriteLine(error);

            if (error != MessageErrors.NoError)
            {
                return BadRequest(error);
            }
            
            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0 && file.Length < 2000000)
                {
                    var filePath = Path.GetTempFileName();
                    Console.WriteLine(filePath);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            
            return new OkResult();
        }
    }
}