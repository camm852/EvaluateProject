namespace EvaluationProjects.Models.Dtos.Request
{
    public class ProjectRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public int StudentId { get; set; }

    }
}
