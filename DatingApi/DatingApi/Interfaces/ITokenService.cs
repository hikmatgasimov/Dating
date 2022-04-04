using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApi.Entities
{
    public interface ITokenService
    {
        string CreatToken(AppUser user);
    }
}
