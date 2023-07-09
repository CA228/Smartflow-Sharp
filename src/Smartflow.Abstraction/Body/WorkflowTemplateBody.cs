using System.ComponentModel.DataAnnotations;

namespace Smartflow.Abstraction.Body
{
    public class WorkflowTemplateBody
    {
        public long Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string CategoryCode
        {
            get;
            set;
        }

        public string CategoryName
        {
            get;
            set;
        }

        public string Source
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public string Memo
        {
            get;
            set;
        }
    }
}
