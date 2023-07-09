using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Smartflow.Abstraction.Body
{
    public class PostContextBody
    {
        public string Message
        {
            get;
            set;
        }

        public List<String> Users
        {
            get;
            set;
        }

        public List<String> Roles
        {
            get;
            set;
        }
    }
}