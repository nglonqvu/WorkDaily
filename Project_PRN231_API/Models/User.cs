using System;
using System.Collections.Generic;

namespace Project_PRN231_API.Models
{
    public partial class User
    {
        public User()
        {
            Tasks = new HashSet<Task>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
