using DatingApi.DTOs;
using DatingApi.Entities;
using DatingApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApi.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string name);
        Task <PagedList<MemberDto>>GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string name);
    }
}
