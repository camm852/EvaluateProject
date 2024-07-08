using EvaluationProjects.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace EvaluationProjects.Models.Dtos.Request
{
    public class UserRequestDto
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
