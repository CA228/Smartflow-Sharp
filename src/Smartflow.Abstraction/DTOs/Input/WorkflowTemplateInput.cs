using System.ComponentModel.DataAnnotations;

namespace Smartflow.Abstraction.DTOs.Input
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

        public int CategoryId
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
