using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ringkey.Common.Models.Messages;
using ringkey.Common.Models.NewFolder;

namespace ringkey.Common.Models
{
    public class Report
    {
        [Key]
        public Guid ReportId {get; set;}
        public Message Message {get; set;}
        public string ReportMessage {get; set;}
    }
}
