using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFirstApi.Data;
using MyFirstApi.Dto;
using MyFirstApi.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyFirstApi.Services
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tuple<int,TokenDto>> LoginUser(UserDto dto)
        {
            try
            {
                var tokenDto = new TokenDto();

                if(dto==null)
                {
                    tokenDto.Message = "Please fill all details";
                  
                    return new Tuple<int,TokenDto>(0, tokenDto);
                }

                var existingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email); 

                if(existingUser==null)
                {

                    tokenDto.Message = "This users doesnt exist.Please login";
              

                    return new Tuple<int, TokenDto>(0,tokenDto);
                }
                //  if(existingUser.Password!=dto.Password)
                //  {
                //      return new Tuple<int, string>(1, "Password Incorrect");
                //  }

                var passwordHasher = new PasswordHasher<string>();

                var verifyPassword = passwordHasher.VerifyHashedPassword(dto.Email, existingUser.Password, dto.Password);

                if(verifyPassword==PasswordVerificationResult.Success)
                {
                    UserDto user = new();
                    user.Name = existingUser.Name;
                    user.Email = existingUser.Email;
                    user.Id = existingUser.Id;
                    var token = GetJwtToken(user);

                    tokenDto.Token = token;
                    tokenDto.Message = "login Successfull";
                   

                    return new Tuple<int, TokenDto>(2, tokenDto);
                }

                else if(verifyPassword==PasswordVerificationResult.SuccessRehashNeeded)
                {
                    UserDto user = new();
                    user.Name = dto.Name;
                    user.Email = existingUser.Email;
                    user.Id = existingUser.Id;
                    var token = GetJwtToken(user);

                    existingUser.Password = PasswordHashing(dto);

                    _context.AccountUsers.Update(existingUser);
                    _context.SaveChanges();

                    tokenDto.Token = string.Empty;
                    tokenDto.Message = "login Successfull , new hash generated";
                    

                    return new Tuple<int, TokenDto>(2, tokenDto);
                    
                }

                else if(verifyPassword==PasswordVerificationResult.Failed)
                {
                    tokenDto.Message = "Password Incorrect";

                    return new Tuple<int, TokenDto>(1, tokenDto);
                }

                tokenDto.Message = "This User Doesnt Exist";

                return new Tuple<int, TokenDto>(0, tokenDto);

            }
            catch(Exception)
            {
                throw;
            }
        }




        private string GetJwtToken(UserDto dto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,dto.Name),
                new Claim(ClaimTypes.Email,dto.Email),
                new Claim(ClaimTypes.NameIdentifier,dto.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2925475dbc1ecbdc5e6998e3da633df706aa7b7e02c128e2ad7ff458835c270f"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "rohan-client",
                audience: "rohan-backend",
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds

                );

            return new JwtSecurityTokenHandler().WriteToken(token);

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
                    Password = PasswordHashing(dto),
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "User registered successfully");
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        private string PasswordHashing(UserDto dto)
        {
            var passwordHasher = new PasswordHasher<string>();

            var hash = passwordHasher.HashPassword(dto.Email, dto.Password);

            return hash;
        }
    }
}
