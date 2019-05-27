using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoDB.FilterParams;
using TodoDB.Helpers;
using TodoDB.Models;

namespace TodoDB.Data.Todo
{
    public class TodoRepoMYSQL : BaseRepoMYSQL, ITodoRepo
    {

        public TodoRepoMYSQL(string connStr, string secretKey) : base(connStr, secretKey) { }

        public Task<Result> TodoList(UserParams userParams, PageParamsForPost pageParams, TodoFilterParams filterParams)
        {
            throw new NotImplementedException();
        }

        public Task<Result> TodoSave(UserParams userParams, TodoItem todoItem)
        {
            throw new NotImplementedException();
        }
    }
}
