using FilmPortal.Controllers;
using FilmPortal.Models;
using FilmPortal.Models.Repo;
using FilmPortal.Tests.TestData;
using FilmsPortalWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests.UnitTests
{
    public class FilmControllerUnitTests
    {
        public readonly ILogger<FilmsController> _logger;
        public FilmControllerUnitTests()
        {
            var factory = new LoggerFactory();
            _logger = factory.CreateLogger<FilmsController>();
        }

        [Fact]
        public async Task GetFilms_ReturnsAllFilms()
        {
            //Arrange

            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetAllFilms()).ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().ToList()
                );

            //Act
            var result = await controller.GetFilms();

            //Assert
            Assert.Collection(result.Value!,
                item => Assert.Equal("Fight Club", item.Title),
                item => Assert.Equal("Terminator", item.Title)
                );
        }

        [Fact]
        public async Task GetFilms_ReturnsNotFound()
        {
            //Arrange

            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetAllFilms()).Returns(
                Task.FromResult<IEnumerable<Film>>(null!)
                );

            //Act
            var result = await controller.GetFilms();

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(1, "Fight Club")]
        [InlineData(2, "Terminator")]
        public async Task GetFilm_ReturnsFilm(int id,string expectedTitle)
        {
            //Arrange

            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetFilmById(It.IsAny<int>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().FirstOrDefault(f=>f.Id ==id)!
                );

            //Act
            var result = await controller.GetFilm(id);

            //Assert
            Assert.Equal(expectedTitle, result.Value!.Title);
        }

        [Fact]
        public async Task GetFilm_ReturnsNotFound()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetFilmById(It.IsAny<int>()))
                .Returns(
                Task.FromResult<Film>(null!)
                );

            //Act
            var result = await controller.GetFilm(id);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutFilm_ReturnsNoContent()
        {
            //Arrange
            int id = 1;
            var filmDto = new FilmCreateDto();
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.UpdateFilm(It.IsAny<int>(),It.IsAny<FilmCreateDto>()))
                .Returns(
                Task.FromResult(true)
                );

            //Act
            var result = await controller.PutFilm(id,filmDto);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task PutFilm_ReturnsNoFound()
        {
            //Arrange
            int id = 1;
            var filmDto = new FilmCreateDto();
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.UpdateFilm(It.IsAny<int>(), It.IsAny<FilmCreateDto>()))
                .Returns(
                Task.FromResult(false)
                );

            //Act
            var result = await controller.PutFilm(id,filmDto);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task PostFilm_ReturnsCreatedAtAction()
        {
            //Arrange
            int id = 1;
            var filmDto = new FilmCreateDto();
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.AddFilm(It.IsAny<FilmCreateDto>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().FirstOrDefault(f => f.Id == id)!
                );

            //Act
            var result = await controller.PostFilm(filmDto);

            //Assert
            Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
        }


        [Fact]
        public async Task PostFilm_ReturnsProblem_CheckNull()
        {
            //Arrange
            var filmDto = new FilmCreateDto();
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.AddFilm(It.IsAny<FilmCreateDto>()))
                .Throws(new TaskCanceledException());

            //Act
            var result = await controller.PostFilm(filmDto);

            //Assert
           Assert.Null(result.Value);
        }

        [Fact]
        public async Task DeleteFilm_ReturnsNoFound()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.DeleteFilm(It.IsAny<int>()))
                .Returns(
                Task.FromResult(false)
                );

            //Act
            var result = await controller.DeleteFilm(id);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFilm_ReturnsNoContent()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IFilmsRepository>();
            var controller = new FilmsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.DeleteFilm(It.IsAny<int>()))
                .Returns(
                Task.FromResult(true)
                );

            //Act
            var result = await controller.DeleteFilm(id);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
        }
    }
}
