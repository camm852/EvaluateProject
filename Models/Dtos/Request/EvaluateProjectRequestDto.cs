using System.ComponentModel.DataAnnotations;

namespace EvaluationProjects.Models.Dtos.Request
{
    public class EvaluateProjectRequestDto
    {
        [Required]
        public bool Approved { get; set; }
        [Required]
        public string FeedBack {  get; set; }
    }
}
