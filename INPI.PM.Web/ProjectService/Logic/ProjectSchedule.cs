using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService.Logic
{
    public class ProjectSchedule
    {
        public ProjectStep Step { get; set; }
        public DateTime EstimatedCompleteTime { get; set; }
        public DateTime? ActualCompleteTime { get; set; }
        public int PersonInCharge { get; set; }
    }
}
