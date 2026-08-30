using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Dto;
using MyFirstApi.IService;

namespace MyFirstApi.Services
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tuple<int,string>> LoginUser(UserDto dto)
        {
            try
            {
                var existingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email); 
                if(existingUser==null)
                {
                    return new Tuple<int, string>(0,"This users doesnt exist.Please login");
                }
                if(existingUser.Password!=dto.Password)
                {
                    return new Tuple<int, string>(1, "Password Incorrect");
                }

                return new Tuple<int, string>(2, "login Successfull");
            }
            catch(Exception)
            {
                return new Tuple<int, string>(3,"Something went wrong");
            }
        }
    }
}
