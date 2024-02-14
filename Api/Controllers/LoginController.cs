using Application.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {        
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        [HttpGet("login")]
        public async Task<ActionResult<LoginDTO>> LoginAsync(string username, string password)
        {
            try
            {
                var result = await _loginService.LoginAsync(username, password);
                if (result == null)
                {
                    return BadRequest("Login was not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginDTO>> RegisterAsync(LoginDTO login)
        {
            try
            {
                var result = await _loginService.RegisterAsync(login);
                return CreatedAtAction("Register",result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<LoginDTO>> UpdateLogin(LoginDTO login)
        {
            try
            {
                await _loginService.UpdateLogin(login);
                var result = await _loginService.LoginAsync(login.Username, login.Password);
                if (result == null)
                    return BadRequest("Login was not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteLogin(int id)
        {
            try
            {
                await _loginService.DeleteLogin(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            
        }
    }
}
