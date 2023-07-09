using System.ComponentModel.DataAnnotations;

namespace Smartflow.Abstraction.Body
{
    /// <summary>
    /// 桥接实
    /// </summary>
    public class StartBody
    {
        [StringLength(2048)]
        public string Comment
        {
            get; set;
        }

    }
}