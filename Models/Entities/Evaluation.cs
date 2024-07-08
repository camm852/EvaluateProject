using System;
using System.Collections.Generic;

namespace EvaluationProjects.Models.Entities;


public partial class Evaluation
{
    public int Id { get; set; }

    public int? AssignmentId { get; set; }

    public int? StatusId { get; set; }

    public bool? Approved { get; set; }

    public string? Feedback { get; set; }

    public DateOnly? EvaluationDate { get; set; }

    public virtual Assignment? Assignment { get; set; }

    public virtual Status? Status { get; set; }
}
