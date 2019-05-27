using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TodoDB.DTOs;
using TodoDB.FilterParams;
using TodoDB.Helpers;
using TodoDB.Library;
using TodoDB.Models;

namespace TodoDB.Data.Todo
{
    public class TodoRepoMSSQL : ITodoRepo
    {
    
        private readonly string _connStr;
        private readonly string _secretKey;

        private readonly IDbConnection _dbConn;
        private readonly Utilities _utilities;

        public TodoRepoMSSQL(string connStr, string secretKey)
        {
            _connStr = connStr;
            _secretKey = secretKey;

            _dbConn = new SqlConnection(_connStr);
            _utilities = new Utilities();
        }

        /// <summary>
        /// to list todos
        /// </summary>
        /// <param name="userParams"></param>
        /// <param name="pageParams"></param>
        /// <param name="filterParams">TodoId, TodoTypeId,TodoStateId, TodoHeader</param>
        /// <returns></returns>
        public async Task<Result> TodoList(UserParams userParams, PageParamsForPost pageParams, TodoFilterParams filterParams)
        {
            var todoListTask = Task.Run(() => {
                return prcTodoList(userParams, pageParams, filterParams);
            });

            // if there is do other stuff

            Result taskResult = await todoListTask;

            return taskResult;        
        }

        public Result prcTodoList(UserParams userParams, PageParamsForPost pageParams, TodoFilterParams filterParams)
        {
            // --- if thers is control before insert  --
            // do here
            // --- /if thers is control before insert  --            


            int rowStart        = (pageParams.PageNumber-1) * pageParams.PageRowCount;
            int rowEnd          = rowStart + pageParams.PageRowCount;          
            string dateFormat   = "dd/mm/yyyy HH24:mi:ss";

            var param = new DynamicParameters();

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(" select T60.* from ");
            queryBuilder.Append(" ( ");
            queryBuilder.Append("       select  ROW_NUMBER() OVER (order by @orderColumnName) rnum, T50.* from ");
            queryBuilder.Append("       (");

            StringBuilder innerQueryBuilder = new StringBuilder();

            innerQueryBuilder.Append("           select T20.* from ");
            innerQueryBuilder.Append("           ( ");            
            innerQueryBuilder.Append("               select T.*, ");
            innerQueryBuilder.Append("                   dbo.fncGetFormattedDateAsStr(T.TodoDate, @dateFormat) as TodoDateStr,");
            innerQueryBuilder.Append("                   TT.TodoType, TS.TodoState ");
            innerQueryBuilder.Append("               from Todo T, TodoType TT, TodoState TS ");
            innerQueryBuilder.Append("               Where 1=1 ");
            innerQueryBuilder.Append("               and T.UserId = @UserId ");
            innerQueryBuilder.Append("               and T.TodoTypeId = TT.TodoTypeId ");
            innerQueryBuilder.Append("               and T.TodoStateId = TS.TodoStateId ");


            //------------- filter parameters ----------------

            if (Convert.ToInt32(filterParams.TodoId) != 0)
            {
                innerQueryBuilder.Append("               and T.TodoId = @TodoId ");

                param.Add("@TodoId", Convert.ToInt32(filterParams.TodoId), dbType: DbType.Int32, direction: ParameterDirection.Input);
            }

         

            if (Convert.ToInt32(filterParams.TodoTypeId) != 0)
            {
                innerQueryBuilder.Append("               and TT.TodoTypeId = @TodoTypeId ");

                param.Add("@TodoTypeId", Convert.ToInt32(filterParams.TodoTypeId), dbType: DbType.Int32, direction: ParameterDirection.Input);
            }



            
            if (Convert.ToInt32(filterParams.TodoStateId) != 0)
            {
                innerQueryBuilder.Append("               and TT.TodoStateId = @TodoStateId ");

                param.Add("@TodoStateId", Convert.ToInt32(filterParams.TodoStateId), dbType: DbType.Int32, direction: ParameterDirection.Input);
            }



            if (filterParams.TodoHeader.Trim() != "")
            {
                innerQueryBuilder.Append("               and T.TodoHeader like '%@TodoHeader%' ");

                param.Add("@TodoHeader", filterParams.TodoHeader.Trim());
            }


            //------------- filter parameters ----------------

            innerQueryBuilder.Append("           ) T20 ");




            queryBuilder.Append(innerQueryBuilder.ToString());

            queryBuilder.Append("           where 1=1 ");
            queryBuilder.Append("       ) T50 ");
            queryBuilder.Append(" ) T60 where rnum>@rowStart and rnum<=@rowEnd ");
          
            

            string query = queryBuilder.ToString();

           
            param.Add("@orderColumnName", pageParams.OrderColumnName);
            param.Add("@dateFormat", dateFormat);
            param.Add("@UserId", userParams.UserId, dbType: DbType.Int32, direction: ParameterDirection.Input);


            param.Add("@rowStart", rowStart, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@rowEnd", rowEnd, dbType: DbType.Int32, direction: ParameterDirection.Input);

         
            try
            {
                string cntQuery = "select count(*) from ( "+ innerQueryBuilder.ToString() +" ) T100 ";                
                int totalRowCount = _dbConn.ExecuteScalar<int>(cntQuery, param);

                
                var TodoList = _dbConn.Query<TodoForListDTO>(query, param, commandType: CommandType.Text).AsList();

                
                return new Result()
                {
                    ResultCode = 1,
                    ResultMessage = "OK",
                    TotalRowCount = totalRowCount,                    
                    obj = TodoList
                };

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

        public async Task<Result> TodoSave(UserParams userParams, TodoItem todoItem)
        {

            var todoSaveTask = Task.Run(() => {
                return prcTodoSave(userParams, todoItem);
            });

            // if there is do other stuff

            Result taskResult = await todoSaveTask;

            return taskResult;
        }


        public Result prcTodoSave(UserParams userParams, TodoItem todoItem)
        {
            // --- if thers is control before insert  --
            // do here
            // --- /if thers is control before insert  --

            string query = "TodoSave"; //stored proc
  
            var param = new DynamicParameters();
            param.Add("@TodoId", todoItem.TodoId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@UserId", userParams.UserId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@TodoTypeId", todoItem.TodoTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@TodoStateId", todoItem.TodoStateId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@TodoHeader", todoItem.TodoHeader);
            param.Add("@TodoText", todoItem.TodoText);
            param.Add("@TodoDateStr", todoItem.TodoDateStr);
            param.Add("@LanguageCode", userParams.LanguageCode);


            try
            {
                //Execute could be used. however we want to get record and result info at the end
                var reader = _dbConn.QueryMultiple(query, param, commandType: CommandType.StoredProcedure);

                //here, there are two single-line lists in multiList
                //1) inserted or updated item as  select --> itemList
                //2) ResultCode and ResultMessage select --> resultList

                var itemList = reader.Read<TodoForListDTO>().AsList();
                var resultList = reader.Read<Result>().AsList();

                var item = new TodoForListDTO();

                var result = resultList[0];

                if (result.ResultCode > 0)
                {
                    item = itemList[0];
                }

                return new Result()
                {
                    ResultCode = result.ResultCode,
                    ResultMessage = result.ResultMessage,
                    obj = item
                };

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


        //public Result prcTodoSave(UserParams userParams, TodoItem todoItem)
        //{
        //    // --- if thers is control before insert  --
        //    // do here
        //    // --- /if thers is control before insert  --

        //    string query = "TodoSave"; //stored proc

        //    //object param = new
        //    //{                
        //    //    TodoId = 0,
        //    //    UserId = userParams.UserId,
        //    //    TodoTypeId  =todoItem.TodoTypeId,
        //    //    TodoStateId  = todoItem.TodoStateId,
        //    //    TodoHeader = todoItem.TodoHeader,
        //    //    TodoText = todoItem.TodoText,
        //    //    TodoDateStr = todoItem.TodoDateStr //  dd/mm/yyyy HH24:mi                
        //    //};

        //    var param = new DynamicParameters();
        //    param.Add("@TodoId", 0, dbType: DbType.Int32, direction: ParameterDirection.Input);
        //    param.Add("@UserId", userParams.UserId, dbType: DbType.Int32, direction: ParameterDirection.Input);
        //    param.Add("@TodoTypeId", todoItem.TodoTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
        //    param.Add("@TodoStateId", todoItem.TodoStateId, dbType: DbType.Int32, direction: ParameterDirection.Input);
        //    param.Add("@TodoHeader", todoItem.TodoHeader);
        //    param.Add("@TodoText", todoItem.TodoText);
        //    param.Add("@TodoDateStr", todoItem.TodoDateStr);
        //    param.Add("@LanguageCode", userParams.LanguageCode);


        //    try
        //    {
        //        //Execute could be used. however we want to get record info at the end
        //        var list = _dbConn.Query<TodoItem>(query, param, commandType: CommandType.StoredProcedure).AsList();

        //        if (list.Count > 0)
        //        {
        //            var user = list[0];

        //            return new Result()
        //            {
        //                ResultCode = 1,
        //                ResultMessage = "OK",
        //                obj = user
        //            };
        //        }
        //        else
        //        {
        //            return new Result()
        //            {
        //                ResultCode = -200,
        //                ResultMessage = "opps!"
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result()
        //        {
        //            ResultCode = -900,
        //            ResultMessage = "err : " + ex.Message
        //        };
        //    }

        //}


    }

}
