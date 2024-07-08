using System.ComponentModel.DataAnnotations;

namespace EvaluationProjects.Models.Dtos.Request
{
    public class UserLoginRequestDto
    {

        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
