using Moq;
using OrderLibrary.Interfaces;
using OrderLibrary.Implemetation;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OrderLibrary.Tests
{
   
    public class OrderTests
    {
        [Fact]
        public void Should_Buy_When_Price_Below_Threshold()
        {
            var serviceMock = new Mock<IOrderService>();
            var order = new Order(serviceMock.Object, 100m);

            bool placedRaised = false;
            order.Placed += _ => placedRaised = true;

            order.RespondToTick("ABC", 90m);

            serviceMock.Verify(s => s.Buy("ABC", 1, 90m), Times.Once);
            Assert.True(placedRaised);
        }

        [Fact]
        public void Should_Not_Buy_When_Price_Above_Threshold()
        {
            var serviceMock = new Mock<IOrderService>();
            var order = new Order(serviceMock.Object, 100m);

            order.RespondToTick("ABC", 150m);

            serviceMock.Verify(s => s.Buy(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void Should_Raise_Error_When_Service_Throws()
        {
            var serviceMock = new Mock<IOrderService>();
            serviceMock
                .Setup(s => s.Buy(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()))
                .Throws(new Exception("Failure"));

            var order = new Order(serviceMock.Object, 100m);

            bool errorRaised = false;
            order.Errored += _ => errorRaised = true;

            order.RespondToTick("ABC", 90m);

            Assert.True(errorRaised);
        }

        [Fact]
        public async Task Should_Only_Buy_Once_When_Called_From_Multiple_Threads()
        {
            var serviceMock = new Mock<IOrderService>();
            var order = new Order(serviceMock.Object, 100m);

            var tasks = new Task[50];

            for (int i = 0; i < 50; i++)
            {
                tasks[i] = Task.Run(() =>
                    order.RespondToTick("ABC", 90m));
            }

            await Task.WhenAll(tasks);

            serviceMock.Verify(s => s.Buy("ABC", 1, 90m), Times.Once);
        }

        [Fact]
        public void Should_Not_Buy_After_Error()
        {
            var serviceMock = new Mock<IOrderService>();
            serviceMock
                .Setup(s => s.Buy(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()))
                .Throws(new Exception());

            var order = new Order(serviceMock.Object, 100m);

            order.RespondToTick("ABC", 90m);
            order.RespondToTick("ABC", 80m);

            serviceMock.Verify(s => s.Buy(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}