using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreManager.Test
{
    public class UserServiceTest
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }
        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        [Fact]
        public void Test_GetStared_ReturnSpecifiedValue()
        {
            ICalculator calculator = Substitute.For<ICalculator>();
            calculator.Multi(1, 2).Returns(2);

            int actual = calculator.Multi(1, 2);
            Assert.Equal(2, actual);
        }
    }

    public interface ICalculator
    {
        int Multi(int x, int y);
    }
}
