using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;
using TodoDB.Models;

namespace TodoDB.Data.Auth
{
    public class AuthRepoMSSQL : BaseRepoMSSQL, IAuthRepo
    {


        public AuthRepoMSSQL(string connStr, string secretKey) :  base(connStr, secretKey) {}

        public async Task<Result> isUserExist(string email)
        {

            return await Task.Run(() =>
            {
                return new Result()
                {

                }; // LoginTmp(email, password);
            });

        }

        public async Task<Result> Login(string email, string password, string languageCode)
        {

            // -- method 1 ----- to wrap sync method ---
            //return await Task.Run(() =>
            //{
            //    return LoginTmp(email, password);
            //});
            // -- /method 1 ----- to wrap sync method ---


            // -- method 2 ----- to continue after calling sync method as async ----
            // do some stuff
            //var LoginTask = Task.Run(() => prcLogin(email, password));

            var LoginTask = Task.Run(   () =>
                                        {
                                            return prcLogin(email, password);
                                        }            
                                    );


            // if there is do other stuff

            //var myOutput = await task;
            Result loginResult = await LoginTask;
            return loginResult;
            // some more stuff
            // -- /method 2 ----- to continue after calling sync method as async ----



            // // -- method 3 ----- another way to wrap sync as async ----
            // User userTmp2 = await Task.FromResult<User>(LoginTmp(email, password));
            // // -- /method 3 ----- another way to wrap sync as async ----



            //// // -- method 4 ----- for multi-independent tasks ---

            //List<Task> taskList = new List<Task>();

            //suppose this is Task1
            //taskList.Add(Task.Factory.StartNew(() =>
            //{
            //    User userTmp = LoginTmp(email, password);   
            //    return userTmp;
            //}));

            //suppose this is Task2
            //taskList.Add(Task.Factory.StartNew(() =>
            //{
            //    User userTmp = LoginTmp(email, password);               
            //    return userTmp;
            //}));


            //await Task.WhenAll(taskList);
            //list can be traversed and each result can be reached

            //return new Result() { ResultCode = 1, ResultMessage = "OK"}; 

            //// // -- /method 4 ----- for multi-independent tasks ---

        }



        public Result prcLogin(string parEmail, string parPassword)
        {
            
            string vchPasswordHash = _utilities.GetSHA512(parPassword,_secretKey);
           
            string query = "select * from Users where Email = @Email and PasswordHash=@PasswordHash";

            object param = new {
                                    Email           = parEmail,
                                    PasswordHash    = vchPasswordHash
                               };


            try
            {                
                var list = _dbConn.Query<User>(query, param, commandType: CommandType.Text).AsList();
                
                if (list.Count > 0)
                {
                    var userFromRepo = list[0];

                    return new Result()
                    {
                        ResultCode = 1,
                        ResultMessage = "OK",
                        obj = userFromRepo
                    };
                }
                else
                {
                    return new Result()
                    {
                        ResultCode = -200,
                        ResultMessage = "NOT found"
                        
                    };
                }
                    

            }
            catch (Exception ex)
            {

                return new Result()
                {
                    ResultCode = -900,
                    ResultMessage = "err : " + ex.Message                   
                };
            }

        }


        public async Task<Result> Register(User parUser, string parPassword)
        {
            var RegisterTask = Task.Run(() => {
                                        return prcRegister(parUser, parPassword);
                                    });

            // do other stuff
            
            Result registerResult = await RegisterTask;

            return registerResult;
            
        }

        public Result prcRegister(User parUser, string parPassword)
        {
            // --- check existing user with email --



            // --- /check existing user with email --


            string vchPasswordHash = _utilities.GetSHA512(parPassword, _secretKey);

            string query = "RegisterOrSaveUser"; //stored proc

            object param = new
            {
                UserId = 0,
                Email = parUser.Email,
                PasswordHash = vchPasswordHash,
                FirstName = parUser.FirstName,
                LastName = parUser.LastName,
                City = parUser.City,
                CountryId = parUser.CountryId
            };


            try
            {
                //Execute could be used. however we want to get user info at the end
                var list = _dbConn.Query<User>(query, param, commandType: CommandType.StoredProcedure).AsList();

                if (list.Count > 0)
                {
                    var user = list[0];

                    return new Result() {
                        ResultCode = 1,
                        ResultMessage = "OK",
                        obj = user
                    };
                }
                else
                {
                    return new Result() {
                        ResultCode = -200,
                        ResultMessage = "opps!"                        
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result()
                {
                    ResultCode = -900,
                    ResultMessage = "err : " + ex.Message                    
                };
            }

        }


    }
}
