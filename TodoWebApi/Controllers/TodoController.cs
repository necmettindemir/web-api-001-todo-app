using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoDB.Data.Todo;
using TodoDB.DTOs;
using TodoDB.FilterParams;
using TodoDB.Helpers;
using TodoDB.Models;

namespace TodoWebApi.Controllers
{
    /// <summary>
    /// Todo operations are in this controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {

        private readonly ITodoRepo _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        /// <summary>
        /// This controller is used for todo crud. It includes Login and Register methods
        /// </summary>
        /// <param name="repo">This is injected repo parameter with pre-parameterized in startup</param>
        /// <param name="config">This is injected config parameter</param>
        /// <param name="mapper">This is injected mapper parameter</param>
        public TodoController
         (
             ITodoRepo repo,
             IConfiguration config,
             IMapper mapper
         )
        {      
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }


        /// <summary>
        /// to save todo
        /// </summary>
        /// <param name="todoForSaveDTO"></param>
        /// <returns></returns>
        [HttpPost("TodoSave")]
        public async Task<IActionResult> TodoSave(TodoForSaveDTO todoForSaveDTO)
        {
            try
            {            
                //---------------------------- save todo ---

                Result result = new Result();

                TodoItem todoItem = new TodoItem();
                _mapper.Map(todoForSaveDTO, todoItem);

                //----------------------- get user info ----

                //UserData : UserId,Email,LanguageCode
                string UserData = User.FindFirst(ClaimTypes.UserData).Value;
                string[] UserParamsItemList = UserData.Split(",");
                UserParams userParams = new UserParams()
                {
                    UserId = Convert.ToInt32(UserParamsItemList[0]),
                    LanguageCode = UserParamsItemList[0]
                };
                //----------------------- /get user info ----
                
                result = await _repo.TodoSave(userParams, todoItem);

                //--------------------------- /save todo ---


                //---------------------------- eval result --

                if (result.ResultCode < 0)
                {                    
                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage
                    });
                }
                else
                {     
                    
                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage,
                        ResultJSONobj = new
                        {
                            todoForListDTO = (TodoForListDTO)result.obj
                        }
                    });
                }

                //---------------------------- /eval result --

            }
            catch (Exception ex)
            {                
                return Ok(new ResultDTO
                {
                    ResultCode = "-500",
                    ResultMessage = ex.Message
                });

            }

        }// TodoSave


        /// <summary>
        /// to list todo
        /// </summary>
        /// <param name="pageParams">pager parameters</param>
        /// <param name="filterParams">filter parameters</param>
        /// <returns></returns>
        [HttpPost("TodoList")]
        public async Task<IActionResult> TodoList(TodoFilterParams filterParams)
        {
            try
            {
                //---------------------------- list todo ---

                Result result = new Result();

                //----------------------- get user info ----

                //UserData : UserId,Email,LanguageCode
                string UserData = User.FindFirst(ClaimTypes.UserData).Value;
                string[] UserParamsItemList = UserData.Split(",");
                UserParams userParams = new UserParams()
                {
                    UserId = Convert.ToInt32(UserParamsItemList[0]),
                    LanguageCode = UserParamsItemList[0]
                };

                //----------------------- /get user info ----

                result = await _repo.TodoList(userParams, filterParams.pageParams, filterParams);

                //--------------------------- /list todo ---


                //---------------------------- eval result --

                if (result.ResultCode < 0)
                {
                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage
                    });
                }
                else
                {

                    PageParamsForReturn pageParamsForReturn = new PageParamsForReturn();
                    _mapper.Map(filterParams.pageParams, pageParamsForReturn);
                    pageParamsForReturn.TotalRowCount = result.TotalRowCount;

                    return Ok(new ResultDTO
                    {
                        ResultCode = result.ResultCode.ToString(),
                        ResultMessage = result.ResultMessage,
                        ResultJSONobj = new
                        {
                            todoForListDTO = (List<TodoForListDTO>)result.obj,
                            pageParams = pageParamsForReturn
                        }
                    });
                }

                //---------------------------- /eval result --

            }
            catch (Exception ex)
            {
                return Ok(new ResultDTO
                {
                    ResultCode = "-500",
                    ResultMessage = ex.Message
                });

            }

        }// TodoList


    }
}
