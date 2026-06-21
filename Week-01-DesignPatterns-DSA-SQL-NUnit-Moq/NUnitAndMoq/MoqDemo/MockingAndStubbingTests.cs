using Moq;
using NUnit.Framework;

[TestFixture]
public class UserServiceTests
{
    [Test]
    public void GetUser_ShouldReturnUserName()
    {
        // Arrange
        var mockRepository = new Mock<IUserRepository>();

        mockRepository
            .Setup(x => x.GetUserName(1))
            .Returns("Surya");

        UserService service =
            new UserService(mockRepository.Object);

        // Act
        string result = service.GetUser(1);

        // Assert
        Assert.AreEqual("Surya", result);
    }
}