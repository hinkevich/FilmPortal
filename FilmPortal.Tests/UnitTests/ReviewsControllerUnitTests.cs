using FilmPortal.Controllers;
using FilmPortal.Models;
using FilmPortal.Models.Repo;
using FilmPortal.Tests.TestData;
using FilmsPortalWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests.UnitTests
{
    public class ReviewsControllerUnitTests
    {
        private readonly ILogger<ReviewsController> _logger;
        public ReviewsControllerUnitTests()
        {
            var factory = new LoggerFactory();
            _logger = factory.CreateLogger<ReviewsController>();
        }


        [Fact]
        public async Task GetReviews_ReturnsAllFilms()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetAllReviews(It.IsAny<int>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().Where(f=>f.Id==id)!.FirstOrDefault()!.Reviews.ToList()
                );

            //Act
            var result = await controller.GetReviews(id);

            //Assert
            Assert.Collection(result.Value!,
                item => Assert.Equal("Ok", item.Title),
                item => Assert.Equal("Not Bad", item.Title)
                );
        }

        [Fact]
        public async Task GetReviews_ReturnsNotFound()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetAllReviews(It.IsAny<int>()))
                .Returns(
                Task.FromResult<IEnumerable<Review>>(null!)
                );

            //Act
            var result = await controller.GetReviews(id);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Theory]
        [InlineData(1,1, "Ok")]
        [InlineData(2,1, "Not Bad")]
        [InlineData(3, 2, "Norm")]
        [InlineData(4, 2, "it's Classicall")]
        public async Task GetReview_ReturnReview(int reviewid, int filmIid, string expectedTitle)
        {
            //Arrange
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetReviewById(It.IsAny<int>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().Where(f => f.Id == filmIid)!.FirstOrDefault()!.Reviews.FirstOrDefault(r=>r.ReviewId==reviewid)!
                );

            //Act
            var result = await controller.GetReview(reviewid);

            //Assert
            Assert.Equal(expectedTitle, result.Value!.Title);
        }


        [Fact]
        public async Task GetReview_ReturnNotFound()
        {
            //Arrange
            int reviewid = 5;
            int filmIid = 1;
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.GetReviewById(It.IsAny<int>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().Where(f => f.Id == filmIid)!.FirstOrDefault()!.Reviews.FirstOrDefault(r => r.ReviewId == reviewid)!
                );

            //Act
            var result = await controller.GetReview(reviewid);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutReview_ReturnNotFound()
        {
            //Arrange
            int reviewid = 5;
            var reviewDto = new ReviewDto();
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.UpdateReview(It.IsAny<int>(),
                It.IsAny<ReviewDto>()))
                .Returns(
                Task.FromResult(false)
                );

            //Act
            var result = await controller.PutReview(reviewid, reviewDto);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutReview_ReturnNoContent()
        {
            //Arrange
            int reviewid = 5;
            var reviewDto = new ReviewDto();
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.UpdateReview(It.IsAny<int>(),
                It.IsAny<ReviewDto>()))
                .Returns(
                Task.FromResult(true)
                );

            //Act
            var result = await controller.PutReview(reviewid, reviewDto);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task PostReview_ReturnCreatedAction()
        {
            //Arrange
            int filmId = 1;
            int reviewid = 1;
            var reviewDto = new ReviewDto();
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.AddReview(It.IsAny<ReviewDto>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().Where(f => f.Id == filmId)!.FirstOrDefault()!.Reviews.FirstOrDefault(r => r.ReviewId == reviewid)!
                );

            //Act
            var result = await controller.PostReview(reviewDto);

            //Assert
            Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostReview_ReturnBadRequest()
        {
            //Arrange
            int filmId = 1;
            int reviewid = 3;
            var reviewDto = new ReviewDto();
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.AddReview(It.IsAny<ReviewDto>()))
                .ReturnsAsync(
                FilmPortalTestData.GetFilmsForTests().Where(f => f.Id == filmId)!.FirstOrDefault()!.Reviews.FirstOrDefault(r => r.ReviewId == reviewid)!
                );

            //Act
            var result = await controller.PostReview(reviewDto);

            //Assert
            Assert.IsAssignableFrom<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task PostReview_ReturnProblem_CheckNull()
        {
            //Arrange
            var reviewDto = new ReviewDto();
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.AddReview(It.IsAny<ReviewDto>()))
                .Throws(new TaskCanceledException());
                

            //Act
            var result = await controller.PostReview(reviewDto);

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task DeleteReview_ReturnNoContent()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.DeleteReview(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = await controller.DeleteReview(id);

            //Assert
            Assert.IsAssignableFrom<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteReview_ReturnNotFound()
        {
            //Arrange
            int id = 1;
            var mockRepo = new Mock<IReviewRepositiry>();
            var controller = new ReviewsController(mockRepo.Object, _logger);
            mockRepo.Setup(repo => repo.DeleteReview(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = await controller.DeleteReview(id);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
    }
}
