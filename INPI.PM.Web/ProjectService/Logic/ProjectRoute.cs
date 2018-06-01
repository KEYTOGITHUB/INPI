using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService.Logic
{
    public class ProjectRoute
    {
        private static string[] _exampleRoute = { "下单", "设计", "下图", "生产", "装配", "自检", "入库", "预验收", "发货", "终验收" };
        public List<ProjectStep> Steps { get; set; }

        public ProjectRoute()
        {
            Steps = new List<ProjectStep>();
            int order = 1;
            foreach (string step in _exampleRoute)
            {
                Steps.Add(new ProjectStep(order++, step));
            }
        }
    }
}
