using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ITNews.Configurations;
using ITNews.DTO;
using ITNews.Services;
using ITNews.Services.Photo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ITNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IHostingEnvironment host;
        private readonly IMapper mapper;
        private readonly PhotoSettings photoSettings;
        private readonly IPhotoService photoService;

        public PhotoController(IHostingEnvironment host, IMapper mapper, IOptionsSnapshot<PhotoSettings> options, IPhotoService photoService)
        {
            this.photoService = photoService;
            photoSettings = options.Value;
            this.mapper = mapper;
            this.host = host;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if (!photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type");

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads", "newsPhoto");
            var photo = await photoService.UploadPhoto(file, uploadsFolderPath);

            return Ok(photo);
        }
        [Route("api/userProfile/Avatar/{userId}")]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar([FromRoute] string userId,IFormFile file)
        {
            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > photoSettings.MaxBytes) return BadRequest("Max file size exceeded");
            if (!photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type");

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads", "Avatars");
            var photo = await photoService.UploadPhoto(file, uploadsFolderPath);

            return Ok(photo);
        }
    }
}