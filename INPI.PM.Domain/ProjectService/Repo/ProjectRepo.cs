using INPI.PM.Infrastructure.DataAccess.RedisAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPI.PM.Domain.ProjectService
{
    public class ProjectRepo
    {
        private static string _key = "projectList";
        private RedisHelper _redisHelper = new RedisHelper();
        private static List<Project> _projectList = new List<Project>();

        public Project GetProject(string guid)
        {
            return _redisHelper.GetModelFromSet<Project>(_key, p => p.GUID == guid);
            //return _projectList.First(p => p.GUID == guid);
        }

        public List<Project> GetProjectsByCustomer(string guid)
        {
            return _redisHelper.GetListFromSet<Project>(_key, p => p.CustomerGuid == guid);
            //return _projectList.Where(p => p.CustomerGuid == guid).ToList();
        }

        public List<Project> GetAllProjects()
        {
            return _redisHelper.GetAllListFromSet<Project>(_key);
            //return _projectList;
        }

        public bool AddProject(Project project)
        {
            return _redisHelper.AddModelToSet<Project>(_key, project);
            //_projectList.Add(project);
            //return true;
        }

        public bool UpdateProject(Project project)
        {
            return _redisHelper.UpdataModelInSet<Project>(_key, p => p.GUID == project.GUID, project);
            //int index = _projectList.FindIndex(p => p.GUID == project.GUID);
            //_projectList[index] = project;
            //return true;
        }

        public bool RemoveProject(string guid)
        {
            return _redisHelper.RemoveModelFromSet<Project>(_key, p => p.GUID == guid);
            //_projectList.Remove(project);
            //return true;
        }
    }
}
