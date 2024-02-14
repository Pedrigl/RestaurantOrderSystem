using Domain.Entities;
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

namespace Infrastructure.Tests.RepositoryTests
{
    [TestClass]
    public class LoginRepositoryTests
    {
        private ILoginRepository _loginRepository;

        [TestInitialize]
        public async Task Initialize()
        {
            RestaurantDbContext context = FakeDbContext.GetFakeDbContext();
            _loginRepository = new LoginRepository(context);

            await _loginRepository.AddAsync(new Login
            {
                DisplayName = "TestInitUser",
                Username = "TestInitUser",
                Password = "TestInitPassword"
            });

            await _loginRepository.AddAsync(new Login
            {
                DisplayName = "TestInitUser2",
                Username = "TestInitUser2",
                Password = "TestInitPassword2"
            });

            await context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task ShouldCreateLogin()
        {
            var newLogin = await _loginRepository.AddAsync(new Login
            {
                DisplayName = "TestUser",
                Username = "TestUser",
                Password = "TestPassword"
            });

            await _loginRepository.SaveAsync();

            var login = await _loginRepository.GetByIdAsync(newLogin.Id);

            login.Should().NotBeNull();
            login.Username.Should().Be("TestUser");
            login.Password.Should().Be("TestPassword");
        }

        [TestMethod]
        public async Task ShouldGetLogin()
        {
            var result = await _loginRepository.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Username.Should().Be("TestInitUser");
            result.Password.Should().Be("TestInitPassword");
        }

        [TestMethod]
        public async Task ShouldGetLoginByUsernameAndPassword()
        {
            var result = _loginRepository.GetWhere(l => l.Username == "TestInitUser" && l.Password == "TestInitPassword");

            result.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task ShouldUpdateLogin()
        {
            var login = await _loginRepository.GetByIdAsync(1);
            login.DisplayName = "TestUserUpdated";
            _loginRepository.Update(login.Id, login);
            await _loginRepository.SaveAsync();

            var updatedLogin = await _loginRepository.GetByIdAsync(1);
            updatedLogin.DisplayName.Should().Be("TestUserUpdated");
        }

        [TestMethod]
        public async Task ShouldDeleteLogin()
        {
            var login = await _loginRepository.GetByIdAsync(1);
            _loginRepository.Delete(login);
            await _loginRepository.SaveAsync();

            var deletedLogin = await _loginRepository.GetByIdAsync(1);
            deletedLogin.Should().BeNull();
        }
    }
}
