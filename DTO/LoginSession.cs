using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class LoginSession
    {
        public User User { get; set; }
        public TokenResponse Tokens { get; set; }
    }
}
