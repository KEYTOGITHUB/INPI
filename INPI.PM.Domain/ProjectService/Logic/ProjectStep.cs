using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService
{
    public class ProjectStep
    {
        public int StepOrder { get; set; }
        public string StepName { get; set; }

        public ProjectStep() { }

        public ProjectStep(int stepOrder, string stepName)
        {
            StepOrder = stepOrder;
            StepName = stepName;
        }
    }
}
