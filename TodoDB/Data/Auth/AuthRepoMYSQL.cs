using System;
using System.Threading.Tasks;
using TodoDB.Models;

namespace TodoDB.Data.Auth
{
    public class AuthRepoMYSQL : BaseRepoMYSQL, IAuthRepo
    {

        public AuthRepoMYSQL(string connStr, string secretKey) : base(connStr, secretKey) { }

        public Task<Result> isUserExist(string email)
        {            
            throw new NotImplementedException();
        }

        public Task<Result> Login(string email, string password, string languageCode)
        {
            //do implemantation for mysql
            throw new NotImplementedException();
        }

        public Task<Result> Register(User user, string password)
        {
            //do implemantation for mysql
            throw new NotImplementedException();
        }
    }
}
