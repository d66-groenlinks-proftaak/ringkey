﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models.Messages
{
    public class ThreadView
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public string Parent { get; set; }
        public long Created { get; set; }
    }
}