using Microsoft.Extensions.DependencyInjection;
using Smartflow.Bussiness.Interfaces;
using Smartflow.Bussiness.Models;
using Smartflow.Bussiness.Queries;
using Smartflow.Bussiness;
using Smartflow.Common;
using Smartflow.Core;
using System.Collections.Generic;

namespace Smartflow.API.Code
{
    public class Ioc
    {
        public static void RegisterService(IServiceCollection services)
        {
            services.AddTransient<IQuery<IList<Category>>, CategoryService>();
            services.AddTransient<IOrganizationService, OrganizationService>();
            services.AddTransient<WorkflowBridgeService>();
            services.AddTransient<WorkflowTemplateService>();
            services.AddTransient<IWorkflowReportService, WorkflowReportService>();
        }
    }
}
