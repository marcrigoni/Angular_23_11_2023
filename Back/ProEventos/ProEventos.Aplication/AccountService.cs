using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProEventos.Aplication.Contracts;
using ProEventos.Aplication.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Repository.Contratos;

namespace ProEventos.Aplication
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AccountService(UserManager<User> userManager,
                                SignInManager<User> signInManager, 
                                IMapper mapper, 
                                IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, true);
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Erro ao verificar password: {ex.Message}");
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserDto>(user);

                    return userToReturn;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Erro ao criar usuário: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userName);
                if (user == null)
                {
                    return null;
                }

                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
                return userUpdateDto;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Erro ao recuperar usuário por nome: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userUpdateDto.UserName);

                if (user == null)
                {
                    return null;
                }

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userRepository.Update<User>(user);

                if (await _userRepository.SaveChangesAsync())
                {
                    var userRetorno = await _userRepository.GetUserByUserNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Erro ao verificar password: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (System.Exception ex)
            {

                throw new Exception($"Erro ao verificar existência de usuário: {ex.Message}");
            }
        }
    }
}