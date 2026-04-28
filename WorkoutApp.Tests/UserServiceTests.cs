using Microsoft.EntityFrameworkCore;
using User_API.Data;
using User_API.DTOs;
using User_API.Models;
using User_API.Service;

namespace WorkoutApp.Tests
{
    public class UserServiceTests
    {
        private UserDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new UserDbContext(options);
        }

        [Fact]
        public async Task CreateUserAsync_ValidDto_ReturnsUser()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new UserService(context);
            var dto = new CreateUserDTO
            {
                FirstName = "Jordan",
                LastName = "Foose",
                UserName = "jfoose",
                Email = "jordan@test.com",
                Password = "Test.1234"
            };

            // Act
            var result = await service.CreateUserAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("jfoose", result.UserName);
            Assert.Equal("Jordan", result.FirstName);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            context.Users.Add(new User
            {
                UserId = 1,
                FirstName = "Jordan",
                LastName = "Foose",
                UserName = "jfoose",
                Email = "jordan@test.com",
                PasswordHash = "hashedpassword"
            });
            await context.SaveChangesAsync();

            var service = new UserService(context);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_ReturnsTrue()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            context.Users.Add(new User
            {
                UserId = 1,
                FirstName = "Jordan",
                LastName = "Foose",
                UserName = "jfoose",
                Email = "jordan@test.com",
                PasswordHash = "hashedpassword"
            });
            await context.SaveChangesAsync();

            var service = new UserService(context);

            // Act
            var result = await service.DeleteUserAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var service = new UserService(context);

            // Act
            var result = await service.DeleteUserAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_UpdatesAndReturnsUser()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            context.Users.Add(new User
            {
                UserId = 1,
                FirstName = "Jordan",
                LastName = "Foose",
                UserName = "jfoose",
                Email = "jordan@test.com",
                PasswordHash = "hashedpassword"
            });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var updateDto = new UpdateUserDTO
            {
                FirstName = "Jordan Updated",
                LastName = "Foose Updated",
                Email = "updated@test.com"
            };

            // Act
            var result = await service.UpdateUserAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jordan Updated", result.FirstName);
            Assert.Equal("updated@test.com", result.Email);
        }
    }
}
