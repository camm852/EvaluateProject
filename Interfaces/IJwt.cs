using EvaluationProjects.Models.Entities;

namespace EvaluationProjects.Interfaces
{
    public interface IJwt
    {
        public string GenerateJWTToken(User user);
    }
}
