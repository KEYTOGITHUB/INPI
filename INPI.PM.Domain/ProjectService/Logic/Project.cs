using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using INPI.PM.Domain.CustomerService;

namespace INPI.PM.Domain.ProjectService
{
    public class Project
    {
        public string GUID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string CustomerGuid { get; set; }
        public string ContactGuid { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string PersonInCharge { get; set; }
        public string Attachment { get; set; }
        public List<ProjectSchedule> ProjectSchedules { get; set; }
        public ProjectRoute ProjectRoute { get; set; }
        public ProjectStep CurrentStep { get; set; }
        public ProjectStep NextStep
        {
            get
            {
                if (CurrentStep == null) return ProjectRoute.Steps.First();
                int order = CurrentStep.StepOrder;
                if (order == ProjectRoute.Steps.Max(p => p.StepOrder))
                {
                    return null;
                }
                else
                {
                    return ProjectRoute.Steps.Single(p => p.StepOrder == order + 1);
                }
            }
        }
        public int OverdueDays
        {
            get
            {
                int tmp;
                if (CurrentStep != null && CurrentStep.StepOrder >= ProjectRoute.Steps.Single(p => p.StepName == "发货").StepOrder)
                {
                    tmp = (ProjectSchedules.Single(p => p.Step.StepName == "发货").ActualCompleteDate.Value - EstimatedDeliveryDate).Days;
                }
                else
                {
                    tmp = (DateTime.Today - EstimatedDeliveryDate).Days;
                }
                return tmp > 0 ? tmp : 0;
            }
        }

        private static ProjectRepo _projectRepo = new ProjectRepo();

        public Project() { }

        public void GoToNextStep(DateTime actualCompleteDate, string personInCharge)
        {
            int index = ProjectSchedules.FindIndex(p => p.Step.StepOrder == NextStep.StepOrder);
            ProjectSchedules[index].ActualCompleteDate = actualCompleteDate;
            ProjectSchedules[index].PersonInCharge = personInCharge;
            CurrentStep = NextStep;
            _projectRepo.UpdateProject(this);
        }
        public DateTime? EstimatedCompleteDate { get; set; }
        public DateTime? ActualCompleteDate { get; set; }
        public DateTime? ProjectCurrentStepEstimatedCompleteDate
        {
            get
            {

                return CurrentStep==null?null:ProjectSchedules.Where(c => c.Step.StepOrder == CurrentStep.StepOrder)
                                                              .FirstOrDefault()
                                                              .EstimatedCompleteDate;
            }
        }
        public int ProjectCurrentStepOverduDays
        {
            get
            {
                if (ProjectCurrentStepEstimatedCompleteDate != null && ProjectCurrentStepEstimatedCompleteDate > DateTime.Now)
                {
                    return (ProjectCurrentStepEstimatedCompleteDate-DateTime.Now).Value.Days;
                }
                return 0;
            }
        }
    }
}
