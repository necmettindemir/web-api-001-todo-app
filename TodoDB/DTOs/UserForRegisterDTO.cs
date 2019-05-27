using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.DTOs
{
    /// <summary>
    /// This object is parameter for RegisterUser
    /// </summary>
    public class UserForRegisterDTO
    {

        public string Email { get; set; }
        public string Password { get; set; }
        //public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }

    }

}
