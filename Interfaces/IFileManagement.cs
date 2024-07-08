using Microsoft.AspNetCore.Mvc;

namespace EvaluationProjects.Interfaces
{
    public interface IFileManagement
    {
        Task<string> UploadFile(string name, Stream stream);
        Task<byte[]> GetStreamFile(string fileName);
    }
}
