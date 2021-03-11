using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models
{
    public class BannedWord
    {
        [Key]
        public string Word { get; set; }
    }
}
