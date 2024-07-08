using System;
using System.Collections.Generic;

namespace EvaluationProjects.Models.Entities;


public partial class Project
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? File { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? StudentId { get; set; }

    public virtual Assignment? Assignment { get; set; }

    public virtual User? Student { get; set; }
}
