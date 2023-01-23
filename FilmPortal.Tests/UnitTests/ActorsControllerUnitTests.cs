using Castle.Core.Logging;
using FilmPortal.Controllers;
using FilmPortal.Models;
using FilmPortal.Models.Repo;
using FilmPortal.Tests.TestData;
using FilmsPortalWebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests.UnitTests
{
    public class ActorsControllerUnitTests
    {
        public readonly ILogger<ActorsController> _logger;
        public ActorsControllerUnitTests()
        {
            var factory = new LoggerFactory();
            _logger = factory.CreateLogger<ActorsController>();
        }
        [Fact]
        public async Task GetActors_ReturnsAllActors()
        {
            //Arrange

            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.GetAllActors()).ReturnsAsync(
                FilmPortalTestData.GetActorsForTests().ToList()
                );

            //Act
            var result = await controller.GetActors();

            //Assert
            Assert.Collection(result.Value!,
                item => Assert.Equal("Brad", item.FirstName),
                item => Assert.Equal("Edward", item.FirstName),
                item => Assert.Equal("Nicole", item.FirstName)
                );

        }

        [Fact]
        public async Task GetActors_ReturnsEmtyList()
        {
            //Arrange
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.GetAllActors()).ReturnsAsync(
                new List<Actor>()
                );

            //Act
            var result = await controller.GetActors();

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(1, "Brad")]
        [InlineData(2, "Edward")]
        [InlineData(3, "Nicole")]
        public async Task GetActor_ReturnsActor(int id, string expectedName)
        {
            //Arrange
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.GetActor(It.IsAny<int>())).ReturnsAsync(
                FilmPortalTestData.GetActorsForTests().FirstOrDefault(a => a.Id == id)
                );

            //Act
            var result = await controller.GetActor(id);

            //Assert
            Assert.Equal(expectedName, result.Value!.FirstName);
        }

        [Fact]
        public async Task PutActor_ReturnsNoContent()
        {
            //Arrange
            var actor = new ActorCreateDto();
            int id = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.UpdateActor(
                It.IsAny<int>()
                , It.IsAny<ActorCreateDto>()
                )).Returns(Task.FromResult(true));

            //Act
            var result = await controller.PutActor(id, actor);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task PutActor_ReturnsNotFound()
        {
            //Arrange
            var actor = new ActorCreateDto();
            int id = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.UpdateActor(
                It.IsAny<int>()
                , It.IsAny<ActorCreateDto>()
                )).Returns(Task.FromResult(false));

            //Act
            var result = await controller.PutActor(id, actor);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task PutActor_InputInvalidId_ReturnsBAdRequest(int id)
        {
            //Arrange
            var actor = new ActorCreateDto();
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.UpdateActor(
                It.IsAny<int>()
                , It.IsAny<ActorCreateDto>()
                )).Returns(Task.FromResult(true));

            //Act
            var result = await controller.PutActor(id, actor);

            //Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }


        [Fact]
        public async Task PutActor_InputNull_ReturnsBAdRequest()
        {
            //Arrange
            int id = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.UpdateActor(
                It.IsAny<int>()
                , It.IsAny<ActorCreateDto>()
                )).Returns(Task.FromResult(true));

            //Act
            var result = await controller.PutActor(id, null!);

            //Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateFreeActor_ReturnsCreateAtActionResult()
        {
            //Arrange
            var actor = new ActorCreateDto();
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.CreateFreeActor(
                 It.IsAny<ActorCreateDto>()
                )).ReturnsAsync(new Actor());

            //Act
            var result = await controller.CreateFreeActor(actor);

            //Assert
            Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task CreateFreeActor_ReturnsProblem_CheckNull()
        {
            //Arrange
            var actor = new ActorCreateDto();
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.CreateFreeActor(
                 It.IsAny<ActorCreateDto>()
                )).Returns(Task.FromResult<Actor>(null!));

            //Act
            var result = await controller.CreateFreeActor(actor);

            //Assert
            Assert.Null(result.Value);
        }


        [Fact]
        public async Task AddActorToFilm_ReturnsCreateAtActionResult()
        {
            //Arrange
            int actorId = 1;
            int filmId = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.BindActorWithFilm(
                 It.IsAny<int>(),
                  It.IsAny<int>()
                )).ReturnsAsync(new Actor());

            //Act
            var result = await controller.AddActorToFilm(filmId, actorId);

            //Assert
            Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task AddActorToFilm_ReturnsNotFoundResult()
        {
            //Arrange
            int actorId = 1;
            int filmId = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.BindActorWithFilm(
                 It.IsAny<int>(),
                  It.IsAny<int>()
                )).Returns(Task.FromResult<Actor>(null!));

            //Act
            var result = await controller.AddActorToFilm(filmId, actorId);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddActorToFilm_ReturnsProblem_CheckNull()
        {
            //Arrange
            int actorId = 1;
            int filmId = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.BindActorWithFilm(
                 It.IsAny<int>(),
                  It.IsAny<int>()
                )).Throws<TaskCanceledException>();

            //Act
            var result = await controller.AddActorToFilm(filmId, actorId);

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task CreateActorAndBindWithFilm_ReturnsCreateAtActionResult()
        {
            //Arrange
            int filmId = 1;
            var actor = new ActorCreateDto();
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.CreateAndBindActorWithFilm(
                 It.IsAny<int>(),
                  It.IsAny<ActorCreateDto>()
                )).ReturnsAsync(new Actor());

            //Act
            var result = await controller.CreateActorAndBindWithFilm(filmId, actor);

            //Assert
            Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task CreateActorAndBindWithFilm_ReturnsNotFoundResult()
        {
            //Arrange
            int filmId = 1;
            var actor = new ActorCreateDto();
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.CreateAndBindActorWithFilm(
                 It.IsAny<int>(),
                  It.IsAny<ActorCreateDto>()
                )).Returns(Task.FromResult<Actor>(null!));

            //Act
            var result = await controller.CreateActorAndBindWithFilm(filmId, actor);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateActorAndBindWithFilm_ReturnsReturnsProblem_CheckNull()
        {
            //Arrange
            int filmId = 1;
            var actor = new ActorCreateDto();
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.CreateAndBindActorWithFilm(
                 It.IsAny<int>(),
                  It.IsAny<ActorCreateDto>()
                )).Throws<TaskCanceledException>();

            //Act
            var result = await controller.CreateActorAndBindWithFilm(filmId, actor);

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task DeleteActor_ReturnsNoContent()
        {
            //Arrange
            int actorId = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.DeleteActor(It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await controller.DeleteActor(actorId);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteActor_ReturnsNotFound()
        {
            //Arrange
            int actorId = 1;
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.DeleteActor(It.IsAny<int>()))
                .ReturnsAsync(false);

            //Act
            var result = await controller.DeleteActor(actorId);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task DeleteActor_ReturnsBadRequest(int actorId)
        {
            //Arrange
            var mockActorsRepo = new Mock<IActorsRepository>();
            var controller = new ActorsController(mockActorsRepo.Object, _logger);
            mockActorsRepo.Setup(repo => repo.DeleteActor(It.IsAny<int>()))
                .ReturnsAsync(true);

            //Act
            var result = await controller.DeleteActor(actorId);

            //Assert
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }
    }
}
