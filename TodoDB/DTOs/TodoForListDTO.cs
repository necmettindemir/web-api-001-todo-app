using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.DTOs
{
    public class TodoForListDTO
    {

        public int TodoId { get; set; }
        public int TodoTypeId { get; set; }
        public string TodoType { get; set; }

        public int TodoStateId { get; set; }
        public string TodoState { get; set; }

        public string TodoHeader { get; set; }
        public string TodoText { get; set; }
        public string TodoDateStr { get; set; } //  yyyy-mm-dd hh24:mi:ss    mssql : 120


    }

}
