using System.ComponentModel.DataAnnotations;

namespace Smartflow.API.Input
{
    public class WorkflowTemplateInput
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

        public string Memo
        {
            get;
            set;
        }
    }
}
