using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Helpers;
using DatingApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public UserRepository(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string name)
        {
            return await _db.AppUser
               .Where(x => x.UserName == name)
               .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)//selecting property we want
               .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _db.AppUser.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
           var  maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>
                (_mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageNumber, userParams.PageSize);
        }

        //public Task<PagedList<IEnumerable<MemberDto>>> GetMembersAsync(UserParams userParams)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IEnumerable<AppUser>> GetUserAsync(string name)
        {
            return await _db.AppUser
                .Include(p=>p.Photos).ToListAsync();
        }

        public Task<IEnumerable<AppUser>> GetUserAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _db.AppUser.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await _db.AppUser
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public void Update(AppUser  user)
        {
            _db.Entry(user).State = EntityState.Modified;
        }
    }
}
