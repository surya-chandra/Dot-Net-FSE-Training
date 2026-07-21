using Moq;
using NUnit.Framework;

[TestFixture]
public class UserServiceTests
{
    [Test]
    public void GetUser_ShouldReturnUserName()
    {

        var mockRepository = new Mock<IUserRepository>();

        mockRepository
            .Setup(x => x.GetUserName(1))
            .Returns("Surya");

        UserService service =
            new UserService(mockRepository.Object);

        string result = service.GetUser(1);

        Assert.AreEqual("Surya", result);
    }
}