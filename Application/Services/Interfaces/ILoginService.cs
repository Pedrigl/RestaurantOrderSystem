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
        Task<LoginDTO> LoginAsync(LoginDTO login);
        Task<LoginDTO> RegisterAsync(LoginDTO login);
        Task<LoginDTO> UpdateLogin(LoginDTO login);
        Task DeleteLogin(LoginDTO login);

    }
}
