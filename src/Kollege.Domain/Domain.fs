namespace Kollege.Domain

type ClassNumber =   
| ClassNumber of departmentAbbreviation:string * number:uint16
with 
  override this.ToString() = 
    match this with 
    | ClassNumber (dept,num) -> sprintf "%s%d" dept num

type Season =
| Fall
| Winter
| Spring
| Summer

type Semester = {
  Year: int
  Season: Season
}

type StudentId = StudentId of int

type Student = {
  Id: StudentId
  FirstName:string
  LastName:string
}

type Class = {
  ClassNo:ClassNumber
  Description: string  
}

type StudentClassRegistration = {
  Id: StudentId
}

type RegisteredStudent = {
  StudentId: StudentId
}