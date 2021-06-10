using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models.Messages
{
    public class MessageRating
    {
        [Key]
        public Guid Id { get; set; }
        public Message Message { get; set; }
        public Account Account { get; set; }
        public MessageRatingType Type { get; set; }
    }
}
