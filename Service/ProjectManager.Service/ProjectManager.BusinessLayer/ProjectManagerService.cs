using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Entities;
using ProjectManager.DAL;
using System.Data.Entity;

namespace ProjectManager.BusinessLayer
{
    public class ProjectManagerService : IProjectManagerService
    {

        private IProjectManagerDbContext dbContext;
        public ProjectManagerService(IProjectManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
            //(this.dbContext as DbContext).Configuration.AutoDetectChangesEnabled = false;
        }

        //public ParentTaskModel AddParentTask(ParentTaskModel parentTask)
        //{
        //    dbContext.ParentTasks.Add(parentTask);
        //    if (dbContext.SaveChanges() >= 0)
        //    {
        //        return parentTask;
        //    }
        //    else { return null; }
        //}

        public ProjectModel AddProject(ProjectModel project)
        {
            project.ProjectManager = null;

            //dbContext.Projects.Attach(project);
            dbContext.SetEntityState(project, EntityState.Added);

            //dbContext.Projects.Add(project);
            //if (project.ProjectManager != null)
            //{
            //    dbContext.SetEntityState(project.ProjectManager, EntityState.Unchanged);
            //}
            if (dbContext.SaveChanges() >= 0)
            {
                return project;
            }
            else { return null; }
        }

        public TaskModel AddTask(TaskModel task)
        {
            //if (task.ParentTask != null)
            //{
            //    dbContext.SetEntityState(task.ParentTask, EntityState.Detached);
            //}
            //if (task.User != null)
            //{
            //    dbContext.SetEntityState(task.User, EntityState.Detached);
            //}
            //if (task.Project != null)
            //{
            //    dbContext.SetEntityState(task.Project, EntityState.Detached);
            //}

            task.ParentTask = null;
            task.User = null;
            task.Project = null;

            dbContext.Tasks.Add(task);
            //dbContext.SetEntityState(task, EntityState.Added);

            if (dbContext.SaveChanges() >= 0)
            {
                return task;
            }
            else { return null; }
        }

        public UserModel AddUser(UserModel user)
        {
            dbContext.Users.Attach(user);
            dbContext.SetEntityState(user, EntityState.Added);

            //if (user.Project != null)
            //{
            //    dbContext.SetEntityState(user.Project, EntityState.Unchanged);
            //}
            //if (user.Tasks != null)
            //{
            //    foreach (var task in user.Tasks)
            //    {
            //        dbContext.SetEntityState(task, EntityState.Unchanged);
            //    }
            //}
            if (dbContext.SaveChanges() >= 0)
            {
                return user;
            }
            else { return null; }
        }

        public bool DeleteProject(ProjectModel project)
        {
            dbContext.Projects.Remove(project);
            return dbContext.SaveChanges() == 1;
        }

        public bool DeleteTaks(TaskModel task)
        {
            dbContext.Tasks.Remove(task);
            return dbContext.SaveChanges() == 1;
        }

        public bool DeleteUser(UserModel user)
        {
            dbContext.Users.Remove(user);
            return dbContext.SaveChanges() == 1;
        }

        public ICollection<TaskModel> GetAllTaskForProject(ProjectModel project)
        {
            return dbContext.Tasks.Where(x => x.Project.ProjectId == project.ProjectId).ToList();
        }

        public ICollection<TaskModel> GetParentTasks()
        {
            return dbContext.Tasks.Where(x => x.IsParentTask).ToList();
        }

        public ICollection<ProjectModel> GetProjects()
        {
            return dbContext.Projects.Include(x => x.Tasks).Include(x => x.ProjectManager).ToList();
        }

        public ICollection<TaskModel> GetTasks()
        {
            return dbContext.Tasks.ToList();
        }

        public TaskModel GetTaskById(int taskId)
        {

            return dbContext.Tasks.Where(x => x.TaskId == taskId).Include(x => x.Project).Include(x => x.ParentTask).Include(x => x.User).FirstOrDefault();
        }

        public UserModel GetUserById(int userId)
        {
            return dbContext.Users.Include(x => x.Tasks).Include(x => x.Projects).FirstOrDefault(x => x.UserId == userId);
        }

        public ICollection<UserModel> GetUsers()
        {
            return dbContext.Users.ToList();
        }

        public ProjectModel UpdateProject(ProjectModel project)
        {
            //if (id.IsClosed && !ignoreClosedCheck)
            //{
            //    throw new Exception("You cannot update an closed task");
            //}
            var proj = dbContext.Projects.Find(project.ProjectId);
            //dbContext.Detach(proj);

            //dbContext.SetEntityState(project, EntityState.Modified);
            ////dbContext.SetEntityState(project.ProjectManager, EntityState.Modified);
            //dbContext.Projects.Attach(project);

            if (project.ProjectManager != null)
            {
                dbContext.SetEntityState(project.ProjectManager, EntityState.Detached);
            }
            if (project.Tasks != null)
            {
                foreach (var task in project.Tasks)
                {
                    dbContext.SetEntityState(task, EntityState.Detached);
                }
            }

            //var entry = dbContext.GetEntry(proj);
            //entry.CurrentValues.SetValues(project);

            dbContext.UpdateCurrentValue(proj, project);

            //dbContext.Projects.Attach(entry);

            //dbContext.Entry(id).State = System.Data.Entity.EntityState.Modified;
            if (dbContext.SaveChanges() >= 0)
            {
                return GetProjectById(project.ProjectId);
            }
            else { return null; }
        }

        public TaskModel UpdateTaks(TaskModel task)
        {
            ////if (id.IsClosed && !ignoreClosedCheck)
            ////{
            ////    throw new Exception("You cannot update an closed task");
            ////}
            //dbContext.Tasks.Attach(task);
            //dbContext.SetEntityState(task, EntityState.Modified);
            ////dbContext.Entry(id).State = System.Data.Entity.EntityState.Modified;
            //if (dbContext.SaveChanges() >= 0)
            //{
            //    return dbContext.Tasks.Find(task.TaskId);
            //}
            //else { return null; }


            //if (id.IsClosed && !ignoreClosedCheck)
            //{
            //    throw new Exception("You cannot update an closed task");
            //}
            var OldTask = dbContext.Tasks.Find(task.TaskId);
            //dbContext.Detach(proj);

            //dbContext.SetEntityState(project, EntityState.Modified);
            ////dbContext.SetEntityState(project.ProjectManager, EntityState.Modified);
            //dbContext.Projects.Attach(project);

            if (task.User != null)
            {
                dbContext.SetEntityState(task.User, EntityState.Detached);
            }
            if (task.ChildTasks != null)
            {
                foreach (var childTask in task.ChildTasks)
                {
                    dbContext.SetEntityState(childTask, EntityState.Detached);
                }
            }


            if (task.ParentTask != null)
            {
                dbContext.SetEntityState(task.ParentTask, EntityState.Detached);
            }

            if (task.User != null)
            {
                dbContext.SetEntityState(task.User, EntityState.Detached);
            }

            dbContext.UpdateCurrentValue(OldTask, task);
            //var entry = dbContext.GetEntry(OldTask);
            //entry.CurrentValues.SetValues(task);
            //dbContext.Projects.Attach(entry);

            //dbContext.Entry(id).State = System.Data.Entity.EntityState.Modified;
            if (dbContext.SaveChanges() >= 0)
            {
                return GetTaskById(task.TaskId);
            }
            else { return null; }

        }

        public UserModel UpdateUser(UserModel user)
        {
            //if (id.IsClosed && !ignoreClosedCheck)
            //{
            //    throw new Exception("You cannot update an closed task");
            //}
            dbContext.Users.Attach(user);
            dbContext.SetEntityState(user, EntityState.Modified);
            //dbContext.Entry(id).State = System.Data.Entity.EntityState.Modified;
            if (dbContext.SaveChanges() >= 0)
            {
                return GetUserById(user.UserId);
            }
            else { return null; }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public ProjectModel GetProjectById(int projId)
        {

            return dbContext.Projects.Where(x => x.ProjectId == projId).Include(x => x.ProjectManager).Include(x => x.Tasks).FirstOrDefault();

        }
    }
}
