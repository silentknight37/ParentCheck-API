using Microsoft.AspNetCore.Http;
using ParentCheck.Common;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ParentCheck.Web.Helpers
{
    public class FileUpload
    {
        public static async Task<string> FileUploadToServer(string uploadPath, IFormFile file)
        {
            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var mimeType= MimeTypeMap.GetExtension(file.ContentType);
                var fileName = $"{Guid.NewGuid()}{mimeType}";

                var pathWithFile = Path.Combine(uploadPath, fileName);

                using (var stream = File.Create(pathWithFile))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception e)
            {
                Log.Information($"Error Message : {e.Message}");
                Log.Information($"Error Details : {e.InnerException}");
                return string.Empty;
            }            
        }

        public static async Task FileDeleteFromServer(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                Log.Information($"Error Message : {e.Message}");
                Log.Information($"Error Details : {e.InnerException}");
            }
        }
    }
}
