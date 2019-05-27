using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoDB.FilterParams;
using TodoDB.Helpers;
using TodoDB.Models;

namespace TodoDB.Data.Todo
{
    public interface ITodoRepo
    {
        Task<Result> TodoSave(UserParams userParams, TodoItem todoItem);

        Task<Result> TodoList(UserParams userParams, PageParamsForPost pageParams, TodoFilterParams filterParams);

    }
}
