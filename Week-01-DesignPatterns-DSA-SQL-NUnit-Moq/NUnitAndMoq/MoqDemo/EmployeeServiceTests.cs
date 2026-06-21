using Moq;
using NUnit.Framework;

[TestFixture]
public class EmployeeServiceTests
{
    [Test]
    public void Employee_Name_Test()
    {
        var mockRepo = new Mock<IEmployeeRepository>();

        mockRepo.Setup(x => x.GetEmployeeName())
                .Returns("Surya");

        EmployeeService service =
            new EmployeeService(mockRepo.Object);

        Assert.AreEqual("Surya", service.GetName());
    }
}