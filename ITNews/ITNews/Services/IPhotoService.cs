using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.DTO;
using Microsoft.AspNetCore.Http;

namespace ITNews.Services
{
    public interface IPhotoService
    {
        Task<Photo> UploadPhoto(IFormFile file, string uploadsFolderPath);
    }
}
