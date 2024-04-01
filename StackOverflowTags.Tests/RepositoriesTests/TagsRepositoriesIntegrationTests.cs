using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackOverflowTags.Data;
using StackOverflowTags.Models;
using StackOverflowTags.Repositories;
using System.Net;

namespace StackOverflowTags.Tests.RepositoriesTests
{
    public class TagsRepositoriesIntegrationTests
    {
        [Theory]
        [InlineData(1,50)]
        public async Task TagsRepository_AddTagsFromPage_ShouldGetTagsFromApi(int page, int pageSize)
        {
            var logger = new Logger<TagsRepository>(new LoggerFactory());
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }
            var httpClient = new HttpClient(handler);
            var tagsRepository = new TagsRepository(logger, httpClient);
            var tagList = new List<Tag>();

            await tagsRepository.AddTagsFromPage(page, pageSize, tagList);

            tagList.Should().NotBeNullOrEmpty();
            tagList.Should().BeOfType<List<Tag>>();
            tagList.Should().HaveCountGreaterThan(0);
        }
        [Fact]
        [Isolated]
        public async Task TagsRepository_SaveTagsToTheDatabase_ShouldAddTagsToTheDatabase()
        {
            var logger = new Logger<TagsRepository>(new LoggerFactory());
            var connection = new SqliteConnection("Data Source=tagsDbTests.db");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("Data Source=tagsDbTests.db").Options;

            using(var context = new AppDbContext(options))
            {
                var tagsRepository = new TagsRepository(context);
                var tags = new List<Tag>
                {
                    new Tag { Id = 1, Name = "js", Count = 33, Percentage = 33.33, Has_Synonyms = false, Is_Moderator_Only = false, Is_Required = false, Synonyms = new() },
                    new Tag { Id = 2, Name = "c#", Count = 33, Percentage = 33.33, Has_Synonyms = false, Is_Moderator_Only = false, Is_Required = false, Synonyms = new() },
                    new Tag { Id = 3, Name = "python", Count = 33, Percentage = 33.33, Has_Synonyms = false, Is_Moderator_Only = false, Is_Required = false, Synonyms = new() }
                };

                await tagsRepository.SaveTagsToTheDatabase(tags);

                context.Tag.Should().HaveCount(tags.Count);
                foreach (var tag in tags)
                {
                    context.Tag.Should().ContainEquivalentOf(tag);
                }
            }
            connection.Close();
        }
        [Fact]
        [Isolated]
        public async Task TagsRepository_GetTagsFromTheDatabase_ReturnsAllTagsFromTheDatabase()
        {
            var logger = new Logger<TagsRepository>(new LoggerFactory());
            var connection = new SqliteConnection("Data Source=tagsDbTests.db");
            connection.Open();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("Data Source=tagsDbTests.db").Options;

            using(var context = new AppDbContext(options))
            {
                var tagsRepository = new TagsRepository(context);
                var tags = new List<Tag>
                {
                    new Tag { Id = 1, Name = "js", Count = 33, Percentage = 33.33, Has_Synonyms = false, Is_Moderator_Only = false, Is_Required = false, Synonyms = new() },
                    new Tag { Id = 2, Name = "c#", Count = 33, Percentage = 33.33, Has_Synonyms = false, Is_Moderator_Only = false, Is_Required = false, Synonyms = new() },
                    new Tag { Id = 3, Name = "python", Count = 33, Percentage = 33.33, Has_Synonyms = false, Is_Moderator_Only = false, Is_Required = false, Synonyms = new() }
                };

                await tagsRepository.SaveTagsToTheDatabase(tags);
                var tagsFromDb = await tagsRepository.GetTagsFromTheDatabase();

                tagsFromDb.Should().HaveCount(context.Tag.Count());
                foreach (var tag in tags)
                {
                    context.Tag.Should().ContainEquivalentOf(tag);
                }
            }
            connection.Close();
        }
    }
}
