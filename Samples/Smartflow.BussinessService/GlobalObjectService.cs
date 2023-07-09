using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.BussinessService
{
    public class GlobalObjectService
    {
        public static IConfiguration Configuration
        {
            get;
            set;
        }
    }
}
