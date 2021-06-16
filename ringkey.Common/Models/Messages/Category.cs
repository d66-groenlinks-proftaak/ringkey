using System;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models.Messages
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Icon { get; set; }
        public bool Hidden { get; set; }
    }
}
