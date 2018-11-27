import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { URLSearchParams } from 'url';
import { HttpParams } from '@angular/common/http';
import {environment} from 'src/environments/environment';

import { TaskModel } from '../model/task-model';
import { ProjectModel } from '../model/project-model';
//import { ParentTaskModel } from '../model/parent-task-model';
import { UserModel } from '../model/user-model';
import { IPmApiService } from './pm-api.service-interface';


@Injectable()
export class PmApiService extends IPmApiService {
    getTaskById(id: number): Observable<TaskModel> {
        return this.httpService.get<TaskModel>( environment.ApiService+"/Task/GetTaskById/"+id);
    }
    getTasks(): Observable<TaskModel[]> {
        return this.httpService.get<TaskModel[]>( environment.ApiService+"/Task/GetTasks");
    }
    getParentTasks(): Observable<TaskModel[]> {
        return this.httpService.get<TaskModel[]>( environment.ApiService+"/Task/GetParentTasks");
    }
    AddTask(task: TaskModel) {
        return this.httpService.post(environment.ApiService+"/Task/AddTask", task);
    }
    UpdateTask(task: TaskModel): Observable<any> {
        return this.httpService.post(environment.ApiService+"/Task/UpdateTask", task);
    }
    DeleteTask(task: TaskModel): Observable<any> {
        return this.httpService.delete(environment.ApiService+"/Task/DeleteTask/"+task.TaskId);
    }

    constructor(private httpService: HttpClient) { 
        super();
      }

    getUsers(): Observable<UserModel[]> {
        return this.httpService.get<UserModel[]>( environment.ApiService+"/User/GetUsers");
    }
    AddUser(user: UserModel) {
        return this.httpService.post(environment.ApiService+"/User/AddUser", user);
    }
    UpdateUser(user: UserModel): Observable<any> {
        return this.httpService.post(environment.ApiService+"/User/UpdateUser", user);
    }
    DeleteUser(user: UserModel): Observable<any> {
        return this.httpService.delete(environment.ApiService+"/User/DeleteUser/"+user.UserId);
    }

    getProjects(): Observable<ProjectModel[]> {
        return this.httpService.get<ProjectModel[]>( environment.ApiService+"/Project/GetProjects");
    }

    AddProject(project: ProjectModel) {
        return this.httpService.post(environment.ApiService+"/Project/AddProject", project);
    }
    UpdateProject(project: ProjectModel): Observable<any> {
        return this.httpService.post(environment.ApiService+"/Project/UpdateProject", project);
    }

  }
  