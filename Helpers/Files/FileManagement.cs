using EvaluationProjects.Interfaces;
using EvaluationProjects.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EvaluationProjects.Helpers.Files
{
    public class FileManagement : IFileManagement
    {
        public async Task<string> UploadFile(string name, Stream stream)
        {
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", name);

            using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStreamOutput);
            }

            return name;
        }

        public async Task<byte[]> GetStreamFile(string fileName)
        {

            string _filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            var filePath = Path.Combine(_filesDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            if (fileBytes == null || fileBytes.Length == 0)
            {
                return null;
            }
            return fileBytes;
        }
    }
}
