using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Dto;
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
                    return NotFound(result.Item2);
                }
                if(result.Item1==1)
                {
                    return BadRequest(result.Item2);
                }
                
                return Ok(result.Item2);
                
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
