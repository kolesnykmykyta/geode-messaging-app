using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Chat : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public string ChatOwnerId { get; set; } = string.Empty;

        public User? ChatOwner { get; set; }
    }
}
