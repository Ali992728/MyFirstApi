using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Dto;
using MyFirstApi.GenericResponse;
using MyFirstApi.IService;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserDto userDto)
        {
            try
            {
                var result = await _authService.LoginUser(userDto);
                if(result.Item1==0)
                {
                    return NotFound(ResponseResult<string>.Failure(null,result.Item2));
                }
                if(result.Item1==1)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                
                return Ok(ResponseResult<string>.Success(null, result.Item2));
                
            }
            catch(Exception)
            {
                throw;
            }
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDto userDto)
        {
            try
            {
                var result = await _authService.RegisterUser(userDto);
            }
            catch(Exception)
            {

            }
        }
    }
}
