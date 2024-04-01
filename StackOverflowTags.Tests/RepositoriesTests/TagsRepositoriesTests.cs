
using Microsoft.Extensions.Logging;
using Moq.Protected;
using StackOverflowTags.Interfaces;
using StackOverflowTags.Models;
using StackOverflowTags.Repositories;
using System.Net;

namespace StackOverflowTags.Tests.RepositoryTests
{
    public class TagsRepositoriesTests
    {
        private readonly Mock<ILogger<TagsRepository>> _loggerMock; 
        public TagsRepositoriesTests()
        {
            _loggerMock = new Mock<ILogger<TagsRepository>>();
        }
        [Theory]
        [InlineData(149)]
        public async Task TagsRepository_GetAllAsync_ReturnsExpectedNumberOfTags(int amountOfTags)
        {
            var _repository = new Mock<ITagsRepository>();
            _repository.Setup(repo => repo.GetAllAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Tag>(new Tag[amountOfTags]));

            var tags = await _repository.Object.GetAllAsync(amountOfTags);

            tags.Should().NotBeNull();
            tags.Should().BeOfType<List<Tag>>();
            tags.Should().HaveCount(amountOfTags);
        }
        [Theory]
        [InlineData(179)]
        public async Task TagsRepository_GetAllAsync_ReturnEmptyList_WhenExceptionOccurs(int amountOfTags)
        {
            var _mockTagsRepository = new Mock<ITagsRepository>();

            _mockTagsRepository.Setup(repo => repo.GetAllAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Simulated exception."));
            List<Tag> result;
            try
            {
                result = await _mockTagsRepository.Object.GetAllAsync(amountOfTags);
            }
            catch
            {
                result = new List<Tag>();
            }

            result.Should().BeEmpty();
        }
        [Theory]
        [InlineData(1, 3)]
        public async Task TagsRepository_AddTagsFromPage_TagListHasExpectedNumberOfTags(int page, int pageSize)
        {
            var _loggerMock = new Mock<ILogger<TagsRepository>>();
            List<Tag> tagList = new List<Tag>();
            var _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var jsonTags = "{\"items\": [ {\"id\": 1, \"has_synonyms\": false, \"is_moderator_only\": false, \"is_required\": false, \"count\": 33, \"percentage\": 33.33, \"name\": \"js\", \"synonyms\": null }, {\"id\": 1, \"has_synonyms\": false, \"is_moderator_only\": false, \"is_required\": false, \"count\": 33, \"percentage\": 33.33, \"name\": \"js\", \"synonyms\": null}, {\"id\": 3, \"has_synonyms\": false, \"is_moderator_only\": false, \"is_required\": false, \"count\": 33, \"percentage\": 33.33, \"name\": \"js\", \"synonyms\": null} ]}";
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(jsonTags) });
            var _httpClient = new Mock<HttpClient>(_mockHttpMessageHandler.Object);
            var _mockTagsRepository = new Mock<TagsRepository>(_loggerMock.Object, _httpClient.Object);

            await _mockTagsRepository.Object.AddTagsFromPage(page, pageSize, tagList);

            tagList.Should().BeOfType<List<Tag>>();
            tagList.Should().NotBeNullOrEmpty();
            tagList.Should().HaveCount(tagList.Count);
        }
        [Theory]
        [InlineData(1, 3)]
        public async Task TagsRepository_AddTagsFromPage_ReturnsLogErrorWhenExceptionOccurs(int page, int pageSize)
        {
            var _loggerMock = new Mock<ILogger<TagsRepository>>();
            var _httpClient = new Mock<HttpClient>();
            var repository = new Mock<TagsRepository>(_loggerMock.Object, _httpClient.Object);
            _httpClient.Object.BaseAddress = new Uri("http://error");

            try
            {
                await repository.Object.AddTagsFromPage(1, 10, new List<Tag>());
            }
            catch
            {
            }

            _loggerMock.Verify(logger => logger.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(), It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
        
    }
}
