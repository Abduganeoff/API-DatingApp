
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp_Backend.DTOs.Request;
using DatingApp_Backend.DTOs.Response;
using DatingApp_Backend.Helpers;
using DatingApp_Backend.Models;
using DatingApp_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;

namespace DatingApp_Backend.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account
                (
                    _cloudinaryConfig.Value.CloudName,
                    _cloudinaryConfig.Value.ApiKey,
                    _cloudinaryConfig.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("{id}", Name = "GetPhoto")]

        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photoForReturn = _mapper.Map<PhotoAfterUploadingResponse>(photoFromRepo);

            return Ok(photoForReturn);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(int userId, [FromForm]PhotoToUploadRequest request)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = request.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParam = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParam);
                }
            }

            request.Url = uploadResult.Uri.ToString();
            request.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(request);

            if (!userFromRepo.Photos.Any(opt => opt.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll())
            {
                var photoForReturn = _mapper.Map<PhotoAfterUploadingResponse>(photo);
                return CreatedAtRoute
                    (
                        "GetPhoto", 
                        new { UserId = userId, id = photo.Id}, 
                        photoForReturn
                    );
            }

            throw new Exception("Photo could not been uploaded1");
        }

        [HttpPut("{id}/setMain")]

        public async Task<IActionResult> SetMain(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            if (!userFromRepo.Photos.Any(opt => opt.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("The photo is already main photo");

            var currentMainPhoto = await _repo.GetMain(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Photo cannot be set to main photo");

        }

    }
}