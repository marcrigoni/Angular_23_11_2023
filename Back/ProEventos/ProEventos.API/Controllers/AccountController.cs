using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Extensions;
using ProEventos.Aplication.Contracts;
using ProEventos.Aplication.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(
            IAccountService accountService, 
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _accountService = accountService;            
        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _accountService.UserExists(userDto.UserName))
                {
                    return BadRequest("Usuário já existe");
                }
                var user = await _accountService.CreateAccountAsync(userDto);
                if (user != null)                
                {
                    return Ok(user);
                }
                return BadRequest("Usuário não criado, tente novamente mais tarde!");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);
                if (user == null)
                {
                    return Unauthorized("Usuário ou senha inválidos");
                }

                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);

                if (!result.Succeeded)
                {
                    return Unauthorized();
                }

                return Ok(new{
                    userName = user.UserName, 
                    PrimeiroNome = user.PriemeiroNome, 
                    token = _tokenService.CreateToken(user)
                });
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]        
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null)
                {
                    return Unauthorized("Usuário Inválido!");
                }                

                var userReturn = await _accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null)
                {
                    return NoContent();
                }
                return Ok(userReturn);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o Usuário. Erro: {ex.Message}");
            }
        }
    }
}