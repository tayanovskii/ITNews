using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITNews.Services.Photo
{
    public interface IPhotoService
    {
        Task<DTO.Photo> UploadPhoto(IFormFile file, string uploadsFolderPath);
        DTO.Photo UploadAvatar(IFormFile file, string uploadsFolderPath);
    }
}
