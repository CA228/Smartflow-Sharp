using System;

namespace Smartflow.Common
{
    public interface IQuery<out T> where T : class
    {
        T Query();
    }
}
