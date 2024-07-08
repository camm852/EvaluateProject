using System;
using System.Collections.Generic;

namespace EvaluationProjects.Models.Entities;

public partial class Assignment
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public int? TeacherId { get; set; }

    public DateOnly? AssignmentDate { get; set; }

    public virtual Evaluation? Evaluations { get; set; }

    public virtual Project? Project { get; set; }

    public virtual User? Teacher { get; set; }
}
