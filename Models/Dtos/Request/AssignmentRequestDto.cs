using System.ComponentModel.DataAnnotations;

namespace EvaluationProjects.Models.Dtos.Request
{
    public class AssignmentRequestDto
    {
        [Required]
        public int ProjectId { get; set; }
        public int? TeacherId { get; set; }
    }
}
