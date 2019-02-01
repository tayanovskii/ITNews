using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITNews.Services.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoStorage photoStorage;
        public PhotoService(IPhotoStorage photoStorage)
        {
            this.photoStorage = photoStorage;
        }
        public async Task<DTO.Photo> UploadPhoto(IFormFile file, string uploadsFolderPath)
        {
            var fileName = await photoStorage.StorePhoto(uploadsFolderPath, file);
            var photo = new DTO.Photo { FileName = fileName };
            return photo;
        }

        public DTO.Photo UploadAvatar(IFormFile file, string uploadsFolderPath)
        {
            var fileName = photoStorage.StoreAvatar(uploadsFolderPath, file);
            var photo = new DTO.Photo { FileName = fileName };
            return photo;
        }
    } 
}

