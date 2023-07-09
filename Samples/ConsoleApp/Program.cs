using System;
using System.Data;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User();
            user.Name = "张三";
            user.Sex = "男";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            DataTable table = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>("["+ json + "]");
            
            Console.WriteLine("Hello World!");
        }
    }


    public class User
    {
        public string Name { get; set; }
        public string Sex { get; set; }
    }
}
