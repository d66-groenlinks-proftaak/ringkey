﻿using ringkey.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Logic.Messages
{
   
    public static class Utility
    {
       public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        } 

        public static MessageErrors CheckMessage(NewMessage message)
        {
            Message newMessage = new Message()
            {
                Author = message.Author,
                Content = message.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Type = MessageType.Thread,
                Title = message.Title
            };
            if (newMessage.Content.Length < 10)
                return MessageErrors.ContentTooShort;
            if (newMessage.Content.Length > 10000)
                return MessageErrors.ContentTooLong;
            if (newMessage.Title.Length < 5)
                return MessageErrors.TitleTooShort;
            if (newMessage.Title.Length > 40)
                return MessageErrors.TitleTooLong;
            if (newMessage.Author.Length < 3)
                return MessageErrors.AuthorTooShort;
            if (newMessage.Author.Length > 100)
                return MessageErrors.AuthorTooLong;
            if (IsValidEmail(message.Email))
                return MessageErrors.InvalidEmail;

            return MessageErrors.NoError;
        }
    }
}
