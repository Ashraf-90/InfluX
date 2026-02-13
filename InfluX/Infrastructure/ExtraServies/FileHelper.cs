using Microsoft.AspNetCore.Http;
using System.IO;

namespace Infrastructure.ExtraServies
{
    public class FileHelper
    {
        // Upload new image
        public static async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            // Ensure folderName never starts with /
            folderName = folderName.TrimStart('/', '\\');

            string uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                folderName
            );

            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        // Delete existing image
        public static void DeleteImage(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            folderName = folderName.TrimStart('/', '\\');

            string uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                folderName
            );

            string filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// يرجع رابط URL الكامل للملف لعرضه في واجهة المستخدم أو الموبايل.
        /// </summary>
        public static string GetFileUrl(HttpRequest request, string relativeFolderPath, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            string baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/{relativeFolderPath.Trim('/')}/{fileName}".Replace("\\", "/");
        }
    }
}
