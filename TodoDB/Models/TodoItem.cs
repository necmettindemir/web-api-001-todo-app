using System;
using System.Collections.Generic;
using System.Text;

namespace TodoDB.Models
{
    public class TodoItem
    {

        public int TodoId { get; set; }
        public int UserId { get; set; }      
        public int TodoTypeId { get; set; }
        public int TodoStateId { get; set; }
        public string TodoHeader { get; set; }
        public string TodoText { get; set; }
        public string TodoDateStr { get; set; } //  dd/mm/yyyy HH24:mi
        
    }
}
