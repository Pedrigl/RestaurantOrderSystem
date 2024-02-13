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
        public LoginService(ILoginRepository loginRepository, IConfigurationManager configuration, IMapper mapper)
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

        public async Task<LoginDTO> LoginAsync(LoginDTO login)
        {
            var loginEntity = _mapper.Map<Login>(login);
            loginEntity.Password = HashPassword(loginEntity.Password);
            var loginResult = _loginRepository.GetWhere(l => l.Username == loginEntity.Username && l.Password == loginEntity.Password).FirstOrDefault();

            if (loginResult != null)
            {
                loginResult.JWtToken = GenerateToken(loginResult);
                loginResult.TokenExpiration = DateTime.UtcNow.AddHours(1);

                _loginRepository.Update(loginResult);
                await _loginRepository.SaveAsync();
            }
            
            return _mapper.Map<LoginDTO>(loginResult);
        }

        public async Task<LoginDTO> RegisterAsync(LoginDTO login)
        {
            var loginEntity = _mapper.Map<Login>(login);
            loginEntity.Password = HashPassword(loginEntity.Password);
            var loginResult = await _loginRepository.AddAsync(loginEntity);
            await _loginRepository.SaveAsync();
            return _mapper.Map<LoginDTO>(loginResult);
        }

        public async Task<LoginDTO> UpdateLogin(LoginDTO login)
        {
            var loginEntity = _mapper.Map<Login>(login);
            var loginResult = _loginRepository.GetWhere(l => l.Username == loginEntity.Username).FirstOrDefault();

            if (loginResult != null)
            {
                loginResult.DisplayName = loginEntity.DisplayName;
                loginResult.AccessLevel = loginEntity.AccessLevel;
                _loginRepository.Update(loginResult);
                await _loginRepository.SaveAsync();
            }
            return _mapper.Map<LoginDTO>(loginResult);
        }

        public async Task DeleteLogin(LoginDTO login)
        {
            var loginEntity = _mapper.Map<Login>(login);
            _loginRepository.Delete(loginEntity);
            await _loginRepository.SaveAsync();
        }

        public string GenerateToken(Login login)
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
