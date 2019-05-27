using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string  PasswordHash { get; set; }

        public string FirstName{ get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public int CountryId { get; set; }
        

    }
}
