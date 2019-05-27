using System;
using System.Collections.Generic;
using System.Text;
using TodoDB.Helpers;

namespace TodoDB.FilterParams
{
   public class TodoFilterParams
    {
        public int TodoId { get; set; }

        public int TodoTypeId{ get; set; }
        public int TodoStateId { get; set; }

        public string TodoHeader { get; set; }

        public PageParamsForPost pageParams { get; set; }

    }
}
