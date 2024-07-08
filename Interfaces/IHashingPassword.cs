namespace EvaluationProjects.Interfaces
{
    public interface IHashingPassword
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);
    }
}
