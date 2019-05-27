using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.DTOs
{
    public class ResultDTO
    {
        public string ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public string resultPkId { get; set; }

        public Object ResultJSONobj { get; set; }

        public ResultDTO()
        {
            ResultCode = "-1";
            ResultMessage = "--";
            resultPkId = "0";
        }

    }

}
