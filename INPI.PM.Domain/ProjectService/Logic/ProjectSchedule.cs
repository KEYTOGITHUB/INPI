using INPI.PM.Domain.UserService;
using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService
{
    public class ProjectSchedule
    {
        public ProjectStep Step { get; set; }
        public DateTime? EstimatedCompleteDate { get; set; }
        public DateTime? ActualCompleteDate { get; set; }
        public string PersonInCharge { get; set; }

        public ProjectSchedule(ProjectStep step, DateTime estimatedCompleteDate, DateTime actualCompleteDate, string personInCharge)
        {
            Step = step;
            EstimatedCompleteDate = estimatedCompleteDate;
            ActualCompleteDate = actualCompleteDate;
            PersonInCharge = personInCharge;
        }

        public ProjectSchedule(ProjectStep step, DateTime? estimatedCompleteDate,string personInCharge)
        {
            Step = step;
            EstimatedCompleteDate = estimatedCompleteDate;
            PersonInCharge = personInCharge;
        }

        public ProjectSchedule(ProjectStep step, DateTime? estimatedCompleteDate)
        {
            Step = step;
            EstimatedCompleteDate = estimatedCompleteDate;
        }

        public ProjectSchedule(ProjectStep step)
        {
            Step = step;
        }
    }
}
