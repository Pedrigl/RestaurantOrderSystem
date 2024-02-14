using Application.DTOs;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data.Contexts;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Data;
using TestUtilities.Data.Configuration;

namespace Application.Tests.ServiceTests
{
    [TestClass]
    public class LoginServiceTests
    {
        private ILoginService _loginService;

        [TestInitialize]
        public async Task Initialize()
        {
            RestaurantDbContext restaurantDbContext = FakeDbContext.GetFakeDbContext();
            ILoginRepository loginRepository = new LoginRepository(restaurantDbContext);
            _loginService = new LoginService(loginRepository, FakeConfiguration.GetFakeConfiguration(), FakeAutoMapper.GetMapper());

            await _loginService.RegisterAsync(new LoginDTO
            {
                DisplayName = "TestUser",
                Username = "TestUser",
                Password = "TestPassword",
                AccessLevel = AccessLevel.Employee
            });
        }

        [TestMethod]
        public async Task LoginShouldBeSuccessfull()
        {
            var result = await _loginService.LoginAsync("TestUser", "TestPassword");

            result.JWtToken.Should().NotBeNull();
        }

        [TestMethod]
        public async Task LoginShouldNotBeSuccessfull()
        {
            var result = await _loginService.LoginAsync("TestUser", "InvalidPassword");

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task RegisterShouldBeSuccessfull()
        {
            var result = await _loginService.RegisterAsync(new LoginDTO
            {
                DisplayName = "TestUser2",
                Username = "TestUser2",
                Password = "TestPassword2",
                AccessLevel = AccessLevel.Employee
            });

            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task UpdateShouldBeSuccessfull()
        {
            var login = await _loginService.LoginAsync("TestUser", "TestPassword");
            login.DisplayName = "TestUserUpdated";

            await _loginService.UpdateLogin(login);

            var result = await _loginService.LoginAsync("TestUser", "TestPassword");

            result.DisplayName.Should().Be("TestUserUpdated");
        }

        [TestMethod]
        public async Task ShouldDeleteLogin()
        {
            await _loginService.DeleteLogin(1);

            var result = await _loginService.LoginAsync("TestUser", "TestPassword");

            result.Should().BeNull();
        }

    }
}
