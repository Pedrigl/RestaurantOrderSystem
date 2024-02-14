using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginDTO> LoginAsync(string username, string password);
        Task<LoginDTO> RegisterAsync(LoginDTO login);
        Task UpdateLogin(LoginDTO login);
        Task DeleteLogin(int loginId);

    }
}
