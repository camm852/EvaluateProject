using System;
using System.Collections.Generic;

namespace EvaluationProjects.Models.Entities;


public partial class Status
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
}
