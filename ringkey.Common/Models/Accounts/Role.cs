using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models
{
    public class Role
    {
        [Key]
        public string Id { get; set; }
        
        public RoleType Type { get; set; }
        public Account Account { get; set; }
    }
}