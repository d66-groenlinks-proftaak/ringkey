﻿using ringkey.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ringkey.Logic.Messages
{
   
    public static class Utility
    {
       public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email.ToLower());
            if (match.Success)
                return true;
            return false;
        } 

        public static MessageErrors CheckMessage(NewMessage message, bool guest = false)
        {
            if (message.Content.Length < 10)
                return MessageErrors.ContentTooShort;
            if (message.Content.Length > 10000)
                return MessageErrors.ContentTooLong;
            if (message.Title.Length < 5)
                return MessageErrors.TitleTooShort;
            if (message.Title.Length > 40)
                return MessageErrors.TitleTooLong;
            
            if (!guest)
            {
                if (message.Author.Length < 2)
                    return MessageErrors.AuthorTooShort;
                if (message.Author.Length > 50)
                    return MessageErrors.AuthorTooLong;
            }

            if (!IsValidEmail(message.Email))
                return MessageErrors.InvalidEmail;

            return MessageErrors.NoError;
        }
    }
}
