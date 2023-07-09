using System;

namespace Smartflow.API.Code
{
    public class CommonMethods
    {
        public static ResultData Response(Object data, int total)
        {
            return new ResultData
            {
                Code = 200,
                Total = total,
                Data=data
            };
        }
    }
}