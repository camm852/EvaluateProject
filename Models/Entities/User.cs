using System;
using System.Collections.Generic;

namespace EvaluationProjects.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Assignment>? Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Project>? Projects { get; set; } = new List<Project>();
}
