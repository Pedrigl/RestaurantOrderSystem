using AutoMapper;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Domain.Entities;
using Application.DTOs;
using Application.Services.Interfaces;
namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly JwtManager _jwtManager;
        private readonly IMapper _mapper;
        public LoginService(ILoginRepository loginRepository, IConfiguration configuration, IMapper mapper)
        {
            _loginRepository = loginRepository;
            _jwtManager = new JwtManager(configuration);
            _mapper = mapper;

        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<LoginDTO> LoginAsync(string username, string password)
        {
            var loginResult = _loginRepository.GetWhere(l => l.Username == username && l.Password == HashPassword(password)).FirstOrDefault();

            if (loginResult != null)
            {
                loginResult.JWtToken = GenerateToken(loginResult);
                loginResult.TokenExpiration = DateTime.Now.AddHours(1);

                _loginRepository.Update(loginResult.Id,loginResult);
                await _loginRepository.SaveAsync();
            }
            
            return _mapper.Map<LoginDTO>(loginResult);
        }

        public async Task<LoginDTO> RegisterAsync(LoginDTO login)
        {
            var loginEntity = _mapper.Map<Login>(login);

            if(!await VerifyIfUserNameIsAvailable(loginEntity))
                throw new Exception("Username is already taken");

            loginEntity.Password = HashPassword(loginEntity.Password);
            var loginResult = await _loginRepository.AddAsync(loginEntity);
            await _loginRepository.SaveAsync();
            return _mapper.Map<LoginDTO>(loginResult);
        }

        private async Task<bool> VerifyIfUserExists(int id)
        {
            var loginResult = await _loginRepository.GetByIdAsync(id);
            return loginResult != null;
        }

        private async Task<bool> VerifyIfUserNameIsAvailable(Login login)
        {
            var usernameCheckLogin = _loginRepository.GetWhere(l => l.Username == login.Username).FirstOrDefault();

            var idCheckLogin = await _loginRepository.GetByIdAsync(login.Id);

            if(usernameCheckLogin == null)
                return true;

            if (usernameCheckLogin.Id == idCheckLogin.Id)
                return true;
                
            //this checks if the username is already taken by another user while not breaking the update if the username is the same

            return false;
        }

        public async Task UpdateLogin(LoginDTO login)
        {
            if (!await VerifyIfUserExists(login.Id))
                throw new Exception("Login not found");

            var loginEntity = _mapper.Map<Login>(login);

            login.Password = HashPassword(login.Password);
            _loginRepository.Update(loginEntity.Id, loginEntity);
            await _loginRepository.SaveAsync();

        }

        public async Task DeleteLogin(int id)
        {
            if (!await VerifyIfUserExists(id))
                throw new Exception("Login not found");

            var loginEntity = await _loginRepository.GetByIdAsync(id);

            _loginRepository.Delete(loginEntity);
            await _loginRepository.SaveAsync();
        }

        private string GenerateToken(Login login)
        {
            var key = _jwtManager.GenerateKey();
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, login.AccessLevel.ToString())

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
