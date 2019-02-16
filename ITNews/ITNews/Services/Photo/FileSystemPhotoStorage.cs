using System;
using System.IO;
using System.Threading.Tasks;
using ITNews.Helpers;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;


namespace ITNews.Services.Photo
{
    public class FileSystemPhotoStorage : IPhotoStorage
    {
        public async Task<string> StorePhoto(string uploadsFolderPath, IFormFile file)
        {
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public string StoreAvatar(string uploadsFolderPath, IFormFile file)
        {
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            //var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid() + ".png";    //for transparent background photo must have .png extension
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var img = Image.Load(file.OpenReadStream()))
            {
                using (Image<Rgba32> destRound = img.Clone(x => x.ConvertToAvatar(new Size(200, 200), 100).BackgroundColor(Rgba32.Transparent)))
                {
                    destRound.Save(filePath);
                }
            }
            return fileName;

        }

        
    }
}
