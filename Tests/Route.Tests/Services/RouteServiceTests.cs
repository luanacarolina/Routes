using Moq;
using Route.Domain.Contracts.Repositories;
using Route.Domain.Entities;
using Route.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Route.Tests.Services
{
    public class RouteServiceTests
    {
        private readonly Mock<IRouteRepository> _repositoryMock;
        private readonly RouteService _service;

        public RouteServiceTests()
        {
            _repositoryMock = new Mock<IRouteRepository>();
            _service = new RouteService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllRoutesAsync_ReturnsAllRoutes()
        {
            var routes = new List<Routes>
        {
            new Routes { Id = 1, Origin = "GRU", Destination = "BRC", Price = 10 },
            new Routes { Id = 2, Origin = "BRC", Destination = "SCL", Price = 5 }
        };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(routes);

            var result = await _service.GetAllRoutesAsync();

            Assert.Equal(2, ((List<Routes>)result).Count);
        }

        [Fact]
        public async Task GetRouteByIdAsync_ReturnsRoute_WhenExists()
        {
            var route = new Routes { Id = 1, Origin = "GRU", Destination = "BRC", Price = 10 };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(route);

            var result = await _service.GetRouteByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("GRU", result.Origin);
        }

        [Fact]
        public async Task GetRouteByIdAsync_ReturnsNull_WhenNotExists()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Routes?)null);

            var result = await _service.GetRouteByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateRouteAsync_AddsAndSavesRoute()
        {
            var route = new Routes { Id = 1, Origin = "GRU", Destination = "BRC", Price = 10 };
            _repositoryMock.Setup(r => r.AddAsync(route)).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.CreateRouteAsync(route);

            _repositoryMock.Verify(r => r.AddAsync(route), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Equal(route, result);
        }

        [Fact]
        public async Task UpdateRouteAsync_ReturnsTrue_WhenRouteExists()
        {
            var route = new Routes { Id = 1, Origin = "GRU", Destination = "BRC", Price = 10 };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(route);
            _repositoryMock.Setup(r => r.UpdateAsync(route)).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var updatedRoute = new Routes { Id = 1, Origin = "GRU", Destination = "SCL", Price = 15 };
            var result = await _service.UpdateRouteAsync(updatedRoute);

            Assert.True(result);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Routes>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateRouteAsync_ReturnsFalse_WhenRouteNotExists()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Routes?)null);

            var route = new Routes { Id = 1, Origin = "GRU", Destination = "SCL", Price = 15 };
            var result = await _service.UpdateRouteAsync(route);

            Assert.False(result);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Routes>()), Times.Never);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteRouteAsync_ReturnsTrue_WhenRouteExists()
        {
            var route = new Routes { Id = 1 };
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(route);
            _repositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.DeleteRouteAsync(1);

            Assert.True(result);
            _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteRouteAsync_ReturnsFalse_WhenRouteNotExists()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Routes?)null);

            var result = await _service.DeleteRouteAsync(1);

            Assert.False(result);
            _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CalculateBestRoute_ReturnsCorrectPathAndPrice()
        {
            var routes = new List<Routes>
        {
            new Routes { Origin = "GRU", Destination = "BRC", Price = 10 },
            new Routes { Origin = "BRC", Destination = "SCL", Price = 5 },
            new Routes { Origin = "SCL", Destination = "ORL", Price = 20 },
            new Routes { Origin = "ORL", Destination = "CDG", Price = 5 }
        };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(routes);

            var result = await _service.CalculateBestRoute("GRU", "CDG");

            Assert.Equal("GRU - BRC - SCL - ORL - CDG ao custo de $40", result);
        }

        [Fact]
        public async Task CalculateBestRoute_ReturnsNotFound_WhenNoRoute()
        {
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Routes>());

            var result = await _service.CalculateBestRoute("ABC", "XYZ");

            Assert.Equal("Route not found.", result);
        }
    }
}