using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.DTOs
{
    public class TodoForSaveDTO
    {
        public int TodoId { get; set; }

        /// <summary>
        /// 1:personal 2:business 9:other
        /// </summary>
        public int TodoTypeId { get; set; }

        /// <summary>
        /// 1: done   2:waiting  3:cancelled    Default:2
        /// </summary>
        public int TodoStateId { get; set; }
        /// <summary>
        /// max 50 chars
        /// </summary>
        public string TodoHeader { get; set; }
        /// max 2000 chars
        public string TodoText { get; set; }
        /// <summary>
        /// format should be yyyy-mm-dd HH:mi:ss. This is mssql date format with 120 
        /// </summary>
        public string TodoDateStr { get; set; } //  dd/mm/yyyy HH24:mi

    }


}
