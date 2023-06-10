import moment from "moment";
export default class TodoDto{
    public id:string
    public task:string
    public isCompleted:boolean
    public createdDate:moment.Moment
    

    constructor(id:string, task:string, isCompleted:boolean, createdDate:Date){
        this.id=id;
        this.task=task,
        this.isCompleted=isCompleted,
        this.createdDate=moment(createdDate);

    }
}