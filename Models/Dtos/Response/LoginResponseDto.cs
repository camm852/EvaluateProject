namespace EvaluationProjects.Models.Dtos.Response
{
    public class LoginResponseDto
    {
        public string Token {  get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
    }
}
