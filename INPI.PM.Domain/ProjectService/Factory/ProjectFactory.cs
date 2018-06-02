using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.ProjectService
{
    public class ProjectFactory
    {
        private static ProjectRepo _projectRepo = new ProjectRepo();

        public List<Project> GetAllProjects()
        {
            return _projectRepo.GetAllProjects();
        }

        public Project GetProject(string guid)
        {
            return _projectRepo.GetProject(guid);
        }

        public List<Project> GetProjectsByCustomer(string guid)
        {
            return _projectRepo.GetProjectsByCustomer(guid);
        }

        public List<Project> GetProjectsByContact(string guid)
        {
            return _projectRepo.GetProjectsByContact(guid);
        }

        public Project AddProject(string projectCode, string projectName, string customerGuid, string contactGuid,
            DateTime createDate, DateTime estimatedDeliveryDate, string personInCharge, string attachment, string currentUser)
        {
            Project project = new Project();
            project.GUID = Guid.NewGuid().ToString();
            project.ProjectCode = projectCode;
            project.ProjectName = projectName;
            project.CustomerGuid = customerGuid;
            project.ContactGuid = contactGuid;
            project.CreateDate = createDate;
            project.EstimatedDeliveryDate = estimatedDeliveryDate;
            project.PersonInCharge = personInCharge;
            project.Attachment = attachment;
            project.ProjectSchedules = new List<ProjectSchedule>();

            var route = new ProjectRoute();
            project.ProjectRoute = route;
            foreach (var step in route.Steps)
            {
                project.ProjectSchedules.Add(new ProjectSchedule(step));
            }
            _projectRepo.AddProject(project);
            return project;
        }

        public bool SaveProject(Project project)
        {
            return _projectRepo.UpdateProject(project);
        }

        public bool RemoveProject(string guid)
        {
            return _projectRepo.RemoveProject(guid);
        }
    }
}
