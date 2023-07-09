using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class Pending<T> where T:class
    {
        public WorkflowTask Task
        {
            get;
            set;
        }

        public T Data { get; set; }
    }
}
