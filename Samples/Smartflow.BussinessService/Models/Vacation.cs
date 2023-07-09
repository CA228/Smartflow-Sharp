using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartflow.BussinessService.Models
{
    public class Vacation
    {
        public string NID
        {
            get;
            set;
        }

        public string InstanceId
        {
            get;
            set;
        }
        public string UID { get; set; }

        public string Name
        {
            get;
            set;
        }

        public int Day
        {
            get;
            set;
        }
        public string Reason
        {
            get;
            set;
        }
        public string VacationType
        {
            get;
            set;
        }
        public DateTime CreateTime
        {
            get;
            set;
        }

        public long TemplateId { get; set; }
    }
}
