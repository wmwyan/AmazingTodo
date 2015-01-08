using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AmazingTodo.Models
{
    public class TodoModel
    {
        [Key]
        public int TodoItemId { get; set; }
        public String Todo { get; set; }
        public byte Priority { get; set; }
        public DateTime? DueDate { get; set; }

    }

   
}