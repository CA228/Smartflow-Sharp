using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartflow.Samples.Models
{
    public class VacationInput
    {
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
    }
}
