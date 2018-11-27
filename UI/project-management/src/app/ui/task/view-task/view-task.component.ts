import { Component, OnInit, AfterViewInit, Input } from "@angular/core";
import {
  FormArray,
  Form,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  FormControl
} from "@angular/forms";

import { Inject } from "@angular/core";
import { switchMap } from "rxjs/internal/operators/switchMap";
import { mergeMap } from "rxjs/internal/operators/mergeMap";
import { filter } from "rxjs/internal/operators/filter";
import { IPmApiService } from "src/app/service/pm-api.service-interface";
import { UserModel } from "src/app/model/user-model";
import { PmServiceBus } from "src/app/service/service_bus";
import { ProjectModel } from "src/app/model/project-model";
import { TaskModel } from "src/app/model/task-model";
import { Router } from "@angular/router";


@Component({
  selector: 'app-view-task',
  templateUrl: './view-task.component.html',
  styleUrls: ['./view-task.component.css']
})
export class ViewTaskComponent implements OnInit {

  searchForm: FormGroup;
  searchInputControl: FormControl;

  @Input()
  tasks: TaskModel[];

  sortBy: String = "StartDate";

  searchUserInputValue:string="";

  ngAfterViewInit(): void {
    //throw new Error("Method not implemented.");
  }

  constructor(
    private service: IPmApiService,
    private serviceBus: PmServiceBus,
    public router: Router
  ) {
    this.initFormsControl();
  }

  ngOnInit() {
    this.loadTaskModel();
    this.serviceBus.ProjectSearchObservable.subscribe(x => {
      this.loadTaskModel();
    });
  }

  private initFormsControl() {
    this.searchInputControl = new FormControl(this.searchUserInputValue);
    

    this.searchForm = new FormGroup({
      searchInputControl: this.searchInputControl
    });

    this.searchForm.valueChanges.subscribe(x=>
      {
        this.searchUserInputValue=this.searchInputControl.value;
      });
  }

  loadTaskModel(): void {
    this.service.getTasks().subscribe(x => {
      this.tasks = x;
    });
  }

  // onEditProject(project: ProjectModel): void {
  //   this.serviceBus.ProjectEditObservable.next(project);
  // }

  // onSuspendProject(project: ProjectModel): void {
  //   project.IsActive=!project.IsActive;
  //   this.service.UpdateProject(project).subscribe(x => {
  //     console.log("Project Suspended...");
  //     //this.router.navigate(["/AddUser"]);
  //     this.loadTaskModel();
  //   });
  // }

  onSort(field: string): void {
    this.sortBy = field;
  }

  OnEditTask(task:TaskModel) {
    this.router.navigate(['EditTask', task.TaskId]);
  }

  OnEndTask(task:TaskModel) {
    task.EndDate=new Date();
    task.IsClosed=true;
    this.service.UpdateTask(task).subscribe(x => {
      console.log("Task Ended...");
      this.loadTaskModel();
    });
  }

  searchProject():void
  {
    
  }
}
