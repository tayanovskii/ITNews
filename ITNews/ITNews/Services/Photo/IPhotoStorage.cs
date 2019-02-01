using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITNews.Services.Photo
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
        string StoreAvatar(string uploadsFolderPath, IFormFile file);
    }
}
