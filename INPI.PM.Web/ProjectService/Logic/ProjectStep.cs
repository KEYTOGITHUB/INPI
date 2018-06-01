using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService.Logic
{
    public class ProjectStep
    {
        public int StepOrder { get; set; }
        public string StepName { get; set; }

        public ProjectStep(int stepOrder, string stepName)
        {
            StepOrder = stepOrder;
            StepName = stepName;
        }
    }
}
