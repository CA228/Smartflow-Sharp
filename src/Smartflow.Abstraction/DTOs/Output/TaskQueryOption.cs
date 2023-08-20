using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Abstraction.DTOs.Output
{
    public class TaskQueryOption:Paging
    {
        public string CategoryId { get; set; }
    }
}
