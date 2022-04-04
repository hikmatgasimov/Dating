using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DatingApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DatingApi.Interfaces;
using DatingApi.DTOs;
using AutoMapper;
using System.Security.Claims;
using DatingApi.Extensions;
using DatingApi.Helpers;

namespace DatingApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(IUserRepository repo
            , IMapper mapper, IPhotoService photoService)
        {
            _repo= repo;
            _mapper = mapper;
            _photoService = photoService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await _repo.GetUserByUserNameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;
            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";

            var users = await _repo.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
        [HttpGet("{username}",Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUsers(string username)
        {
            return await _repo.GetMemberAsync(username);        
        }
        [HttpPut]
        public async Task<ActionResult>UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _repo.GetUserByUserNameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _repo.Update(user);

            if (await _repo.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");         
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult> AddPhoto(IFormFile file)
        {
            var user = await _repo.GetUserByUserNameAsync(User.GetUsername());
           
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _repo.SaveAllAsync()) {

                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            } 

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _repo.GetUserByUserNameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _repo.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _repo.GetUserByUserNameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You can not delete main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (await _repo.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete");
        }
    }
}
