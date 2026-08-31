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
                throw;
            }
        }


        public async Task<Tuple<int,string>> RegisterUser(UserDto dto)
        {
            try
            {
                var existingUser = await _context.AccountUsers.AnyAsync(x => x.Email == dto.Email);

                if(existingUser)
                {
                    return new Tuple<int, string>(0, "This user already exists.please register with new user");
                }

                _context.AccountUsers.Add(new Entities.User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = dto.Password,
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "User registered successfully");
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
