using System;
using System.Collections.Generic;

namespace ASPDOTNETCOREWebAPI_CRUD.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? StudentName { get; set; }

    public string? StudentGender { get; set; }

    public int? Age { get; set; }

    public int? Standerd { get; set; }

    public string? FatherName { get; set; }
}
