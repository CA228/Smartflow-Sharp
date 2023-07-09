using System;
using System.Collections.Generic;
using System.Text;

namespace Smartflow.Samples
{
    public class RequestContent<T> : RequestContent
    {
        public T Data { get; set; }
    }

    public class RequestContent
    {
        public string InstanceId { get; set; }

        public long TaskId { get; set; }

        public string LineId { get; set; }

        public string Message { get; set; }
    }
}
