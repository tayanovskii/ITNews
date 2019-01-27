using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.DTO;
using Microsoft.AspNetCore.Http;

namespace ITNews.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoStorage photoStorage;
        public PhotoService(IPhotoStorage photoStorage)
        {
            this.photoStorage = photoStorage;
        }
        public async Task<Photo> UploadPhoto(IFormFile file, string uploadsFolderPath)
        {
            var fileName = await photoStorage.StorePhoto(uploadsFolderPath, file);
            var photo = new Photo { FileName = fileName };
            return photo;
        }
    }
}

