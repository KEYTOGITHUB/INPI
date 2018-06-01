using System;
using System.Collections.Generic;
using System.Text;

namespace INPI.PM.Domain.ProjectService.Logic
{
    public class Project
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int CustomerId { get; set; }
        public int ContactId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public int PersonInChargeOfContract { get; set; }
        public List<string> Attachment { get; set; }
        public List<ProjectSchedule> ProjectSchedules { get; set; }
        public ProjectRoute ProjectRoute { get; set; }
        public ProjectStep CurrentStep { get; set; }
    }
}
