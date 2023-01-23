using FilmPortal.Models;
using FilmPortal.Tests.FakeRepositories;
using FilmsPortalWebAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FilmPortal.Tests.IntegrationTests
{
    public class ActorsControllerIntegrationTests : IClassFixture<FilmPortalWebApplicationFactory<Program>>
    {
        private readonly FilmPortalWebApplicationFactory<Program> _factory;
        public ActorsControllerIntegrationTests(FilmPortalWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/Actors", HttpStatusCode.OK)]
        [InlineData("/api/Actors/0", HttpStatusCode.BadRequest)]
        [InlineData("0", HttpStatusCode.NotFound)]
        [InlineData("/api/Actors/1", HttpStatusCode.OK)]
        [InlineData("/api/Actors/2", HttpStatusCode.OK)]
        [InlineData("/api/Actors/3", HttpStatusCode.OK)]
        [InlineData("/api/Actors/4", HttpStatusCode.NotFound)]
        public async Task GetRequestTest_CheckStatusCode(string path, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(path);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }


        [Theory]
        [InlineData("/api/Actors/0", HttpStatusCode.BadRequest)]
        [InlineData("/api/Actors/1", HttpStatusCode.NoContent)]
        [InlineData("/api/Actors/2", HttpStatusCode.NoContent)]
        [InlineData("/api/Actors/3", HttpStatusCode.NoContent)]
        [InlineData("/api/Actors/4", HttpStatusCode.NotFound)]
        public async Task PutRequestTest_CheckStatusCode(string path, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var client = _factory.CreateClient();
            var actorCreateDto = new ActorCreateDto
            {
                FirstName = "Tom",
                LastName = "Hanks"
            };
            JsonContent content = JsonContent.Create(actorCreateDto);

            // Act
            var response = await client.PutAsync(path, content);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutRequestTest_InputNullReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            ActorCreateDto actorCreateDto = new ActorCreateDto();
            var expectedStatusCode = HttpStatusCode.BadRequest;
            string path = "/api/Actors/1";
            JsonContent content = JsonContent.Create(actorCreateDto);

            // Act
            var response = await client.PutAsync(path, content);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }


        [Theory]
        [InlineData("/api/Actors", HttpStatusCode.Created, true)]
        [InlineData("/api/Actors", HttpStatusCode.ServiceUnavailable, false)]
        public async Task PostRequestTestAddFreeActor_CheckStatusCode(string path, HttpStatusCode expectedStatusCode, bool switchBehavior)
        {
            // Arrange
            var client = _factory.CreateClient();
            var actorCreateDto = new ActorCreateDto
            {
                FirstName = "Tom",
                LastName = "Hanks"
            };
            JsonContent content = JsonContent.Create(actorCreateDto);

            //Act
            FakeActorsRepository.SwitchResult = switchBehavior;
            var result = await client.PostAsync(path, content);
            Debug.WriteLine("Serge message");

            //Assert
            Assert.Equal(expectedStatusCode, result.StatusCode);
        }

        [Theory]
        [InlineData("/api/Actors/bind?filmId=1&actorId=1", HttpStatusCode.Created, true)]
        [InlineData("/api/Actors/bind?filmId=3&actorId=1",  HttpStatusCode.NotFound, true)]
        [InlineData("/api/Actors/bind?filmId=1&actorId=4",  HttpStatusCode.NotFound, true)]
        [InlineData("/api/Actors/bind?filmId=1&actorId=1",  HttpStatusCode.ServiceUnavailable, false)]
        public async Task PostRequestTestAddActorToFilm_CheckStatusCode(string path, HttpStatusCode expectedStatusCode, bool switchBehavior)
        {
            // Arrange
            var client = _factory.CreateClient();
            JsonContent content = JsonContent.Create(new Actor());

            //Act
            FakeActorsRepository.SwitchResult = switchBehavior;
            var result = await client.PostAsync(path,content);
            Debug.WriteLine("Serge message");

            //Assert
            Assert.Equal(expectedStatusCode, result.StatusCode);
        }

        [Theory]
        [InlineData("/api/Actors/film?filmId=1", HttpStatusCode.Created, true)]
        [InlineData("/api/Actors/film?filmId=3", HttpStatusCode.NotFound, true)]
        [InlineData("/api/Actors/film?filmId=1", HttpStatusCode.ServiceUnavailable, false)]
        public async Task PostRequestTestCreateAndBind_CheckStatusCode(string path, HttpStatusCode expectedStatusCode, bool switchBehavior)
        {
            // Arrange
            var client = _factory.CreateClient();
            var actorCreateDto = new ActorCreateDto
            {
                FirstName = "Tom",
                LastName = "Hanks"
            };
            JsonContent content = JsonContent.Create(actorCreateDto);

            //Act
            FakeActorsRepository.SwitchResult = switchBehavior;
            var result = await client.PostAsync(path, content);

            //Assert
            Assert.Equal(expectedStatusCode, result.StatusCode);
        }

        [Theory]
        [InlineData("/api/Actors/0", HttpStatusCode.BadRequest)]
        [InlineData("/api/Actors/1", HttpStatusCode.NoContent)]
        [InlineData("/api/Actors/2", HttpStatusCode.NoContent)]
        [InlineData("/api/Actors/3", HttpStatusCode.NoContent)]
        [InlineData("/api/Actors/4", HttpStatusCode.NotFound)]
        public async Task DeleteRequestTest_CheckStatusCode(string path, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.DeleteAsync(path);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
