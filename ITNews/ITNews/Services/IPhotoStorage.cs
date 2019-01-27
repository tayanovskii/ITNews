using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITNews.Services
{
    public interface IPhotoStorage
    {
        Task<string> StorePhoto(string uploadsFolderPath, IFormFile file);
    }
}
