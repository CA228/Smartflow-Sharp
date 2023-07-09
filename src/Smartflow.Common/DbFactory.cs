using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;

namespace Smartflow.Common
{
    public sealed class DbFactory
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory _bussinessSessionFactory;

        public static ISessionFactory SessionFactory => _sessionFactory ??= new Configuration()
                    .Configure("hibernate.core.cfg.xml").BuildSessionFactory();

        public static ISessionFactory BussinessSessionFactory => _bussinessSessionFactory ??= new Configuration()
                    .Configure("hibernate.cfg.xml").BuildSessionFactory();

        public static ISession OpenSession()
        {
            return SessionFactory
                  .WithOptions()
                  .Interceptor(new CommandInterceptor())
                  .OpenSession();
        }

        public static ISession OpenBussinessSession()
        {
            return BussinessSessionFactory
                  .WithOptions()
                  .Interceptor(new CommandInterceptor())
                  .OpenSession();
        }
    
        public static ISessionFactory CreateSessionFactory(string connectionString, string providerName)
        {
            var cfg = new Configuration().Configure();
            IDictionary<string, string> connectionProperies= cfg.Properties;
            if (connectionProperies.ContainsKey("connection.connection_string"))
            {
                connectionProperies["connection.connection_string"] = connectionString;
            }
            else
            {
                connectionProperies.Add("connection.connection_string",connectionString);
            }
            if (connectionProperies.ContainsKey("connection.driver_class"))
            {
                connectionProperies["connection.driver_class"] = providerName;
            }
            else
            {
                connectionProperies.Add("connection.driver_class", providerName);
            }
            return cfg.BuildSessionFactory();
        }
    }
}

