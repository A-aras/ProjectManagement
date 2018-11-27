import { TaskModel } from "./task-model";


export interface UserModel {
    UserId: number;
    FirstName     : string;
    LastName: string ;
    EmployeeId: number ;
    //ProjectId: number|null;
    //TaskId     : number|null;
    //Task     : TaskModel |null;
}
