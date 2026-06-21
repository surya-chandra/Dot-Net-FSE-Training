using NUnit.Framework;

namespace NUnitDemo.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        private Calculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [Test]
        public void Add_ShouldReturnCorrectSum()
        {
            int result = calculator.Add(10, 20);

            Assert.AreEqual(30, result);
        }

        [Test]
        public void Subtract_ShouldReturnCorrectDifference()
        {
            int result = calculator.Subtract(20, 5);

            Assert.AreEqual(15, result);
        }

        [Test]
        public void Multiply_ShouldReturnCorrectProduct()
        {
            int result = calculator.Multiply(4, 5);

            Assert.AreEqual(20, result);
        }

        [Test]
        public void Divide_ShouldReturnCorrectQuotient()
        {
            double result = calculator.Divide(20, 4);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void Divide_ByZero_ShouldThrowException()
        {
            Assert.Throws<DivideByZeroException>(
                () => calculator.Divide(10, 0)
            );
        }
    }
}