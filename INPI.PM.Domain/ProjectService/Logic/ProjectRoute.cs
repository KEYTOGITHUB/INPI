using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService
{
    public class ProjectRoute
    {
        public List<ProjectStep> Steps { get; set; }

        public ProjectRoute()
        {
            Steps = new List<ProjectStep>();
            foreach (RouteStep step in Enum.GetValues(typeof(RouteStep)))
            {
                Steps.Add(new ProjectStep((int)step,step.ToString()));
            }
        }
    }

    public enum RouteStep
    {
        下单=1,
        设计=2,
        下图=3,
        生产=4,
        装配=5,
        自检=6,
        入库=7,
        预验收=8,
        发货=9,
        终验收=10
    }
}
