using System;
using System.Collections.Generic;

namespace Project_PRN231_API.Models
{
    public partial class UserTemp
    {
        public int UserTempId { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
