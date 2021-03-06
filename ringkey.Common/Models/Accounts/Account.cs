﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ringkey.Common.Models.Messages;

namespace ringkey.Common.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Biography { get; set; }
        public string ProfilePicture { get; set; }
        public List<Role> Roles { get; set; }
        public List<Report> Reports { get; set; }
        public List<Message> Messages { get; set; }
    }
}