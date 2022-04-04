using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApi.DTOs;
using DatingApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context,ITokenService tokenService,IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmc = new HMACSHA512();


            user.UserName = registerDto.UserName;
            user.PasswordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmc.Key;
           
            _context.AppUser.Add(user);
             await  _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = registerDto.UserName,
                Token = _tokenService.CreatToken(user),
                KnownAs= registerDto.KnownAs,
                Gender = user.Gender
            };
        }
        private async Task<bool>UserExists(string username)
        {
            return await _context.AppUser.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto dto)
        {
            var user = await _context.AppUser
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == dto.UserName);

            if (user == null) return Unauthorized("Invalid username");

            using var hmc = new HMACSHA512(user.PasswordSalt);

            var computeHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreatToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender=user.Gender
            };
        }
    }
}
