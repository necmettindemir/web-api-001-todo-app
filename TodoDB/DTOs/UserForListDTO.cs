using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.DTOs
{
    public class UserForListDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

}
