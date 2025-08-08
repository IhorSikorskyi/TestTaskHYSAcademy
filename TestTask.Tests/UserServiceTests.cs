using TestTask.Services.Users;

namespace TestTask.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService();
        }

        [Fact]
        public void CreateUser_ShouldReturnUserWithIdAndName()
        {
            var userName = "Alice";
            var user = _service.CreateUser(userName);

            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal(userName, user.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CreateUser_ShouldThrowArgumentNullException_WhenNameIsNullOrWhitespace(string invalidName)
        {
            // Arrange
            var service = new UserService();

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => service.CreateUser(invalidName));
            Assert.Contains("name", exception.ParamName, StringComparison.OrdinalIgnoreCase);
        }


        [Fact]
        public void CreateUser_ShouldReturnUsersWithUniqueIdsAndNames()
        {
            // Arrange
            var service = new UserService();
            string userNameA = "Alice";
            string userNameB = "Bob";

            // Act
            var userA = service.CreateUser(userNameA);
            var userB = service.CreateUser(userNameB);

            // Assert
            Assert.NotNull(userA);
            Assert.NotNull(userB);

            Assert.NotEqual(userA.Id, userB.Id); // ïåðåâ³ðÿºìî, ùî id ð³çí³

            Assert.Equal(1, userA.Id);
            Assert.Equal(userNameA, userA.Name);

            Assert.Equal(2, userB.Id);
            Assert.Equal(userNameB, userB.Name);
        }


        [Fact]
        public void GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var service = new UserService();
            var createdUser = service.CreateUser("Bob");

            // Act
            var user = service.GetUserById(createdUser.Id);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(createdUser.Id, user.Id);
            Assert.Equal("Bob", user.Name);
        }

        [Fact]
        public void GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var service = new UserService();

            // Act
            var user = service.GetUserById(999);

            // Assert
            Assert.Null(user);
        }
    }
}