using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartflow.BussinessService
{
    public static class RestServiceExtensions
    {
     
        /// <summary>
        /// https://reactiveui.github.io/refit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T For<T>()
        {
            var jsonSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };

            var setting = new RefitSettings(new NewtonsoftJsonContentSerializer(jsonSetting))
            {
               // HttpMessageHandlerFactory = () => new AuthHeaderHandler()
            };

            return RestService.For<T>(SystemConstraint.CONST_WORKFLOW_URL, setting);
        }
    }
}
