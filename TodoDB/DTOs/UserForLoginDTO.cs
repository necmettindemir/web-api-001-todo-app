using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.DTOs
{
    /// <summary>
    /// This object is parameter for Login
    /// </summary>
    public class UserForLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string LanguageCode { get; set; } // TR , EN , etc

    }

}
