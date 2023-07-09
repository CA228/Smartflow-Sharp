using Microsoft.AspNetCore.Mvc;
using Smartflow.BussinessService.Models;
using Smartflow.BussinessService.Services;
using System;
using System.Collections.Generic;

namespace Smartflow.Samples.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService = new UserService();

        [Route("api/user/{id}/info"), HttpGet]
        public string Get(string id)
        {
            var user = userService.Get(id);
            return ToBase64String(Newtonsoft.Json.JsonConvert.SerializeObject(user));
        }

        [Route("api/user/list"), HttpGet]
        public IList<User> GetUserList()
        {
            var users = userService.GetUserList();
            return users;
        }

        public static string ToBase64String(string s)
        {
            byte[] byteArray = System.Text.UTF8Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(byteArray);
        }
    }
}
