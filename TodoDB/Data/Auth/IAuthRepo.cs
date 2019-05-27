using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoDB.Models;

namespace TodoDB.Data.Auth
{
    public interface IAuthRepo
    {
        Task<Result> Login(string email, string password,string languageCode);

        Task<Result> Register(User user, string password);

        Task<Result> isUserExist(string email);
    }
}
