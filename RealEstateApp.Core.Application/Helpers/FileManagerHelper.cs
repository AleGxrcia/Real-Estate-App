using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Core.Application.Helpers
{
    public static class FileManagerHelper
    {
        public static string UploadFile(IFormFile file, int id, string entity = "", bool isEditMode = false, string imagePath = "")
        {
            string idString = id.ToString();
            return UploadFileCommon(file, idString, entity, isEditMode, imagePath);
        }

        public static string UploadFile(IFormFile file, string id, string entity = "", bool isEditMode = false, string imagePath = "")
        {
            return UploadFileCommon(file, id, entity, isEditMode, imagePath);
        }

        private static string UploadFileCommon(IFormFile file, string id, string entity = "", bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode && file == null)
            {
                return imagePath;
            }

            //get directory path
            string basePath = $"/Images/{entity}/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if no exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //get file path
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string filename = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, filename);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImageName = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImageName);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{filename}";
        }

        public static void DeleteFile(int id, string entity = "")
        {
            string idString = id.ToString();
            DeleteFileCommon(idString, entity);
        }

        public static void DeleteFile(string id, string entity = "")
        {
            DeleteFileCommon(id, entity);
        }

        private static void DeleteFileCommon(string id, string entity = "")
        {
            //get directory path
            string basePath = $"/Images/{entity}/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }
        }
    }
}
