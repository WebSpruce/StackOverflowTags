using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Data;
using StackOverflowTags.Helper;
using StackOverflowTags.Interfaces;
using StackOverflowTags.Models;
using System.Net;
using System.Text.Json;

namespace StackOverflowTags.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly HttpClient _client;
        private readonly ILogger<TagsRepository> _logger;
        private readonly AppDbContext _context;
        public TagsRepository(ILogger<TagsRepository> logger, AppDbContext context)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            _client = new HttpClient(handler);
            _logger = logger;
            _context = context;

            _context.Database.EnsureCreatedAsync().Wait();
        }
        public TagsRepository(ILogger<TagsRepository> logger, HttpClient client)
        {
            _client = client;
            _logger = logger;
        }
        public TagsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tag>> GetAllAsync(int amountOfTags)
        {
            try 
            { 
                List<Tag> tagList = new List<Tag>();
                int fullPages = amountOfTags / 100;
                int remainder = amountOfTags % 100;
                for (int i = 1; i <= fullPages; i++)
                {
                    await AddTagsFromPage(i, 100, tagList);
                }
                if(remainder > 0)
                {
                    await AddTagsFromPage(fullPages + 1, remainder, tagList);
                }
                if(tagList.Count > amountOfTags)
                {
                    tagList = tagList.Take(amountOfTags).ToList();
                }
                return tagList;
            }
            catch(Exception ex)
            {
                _logger.LogError($"TagsRepository GetAllAsync error: {ex}");
                return new List<Tag>();
            }
        }
        public async Task AddTagsFromPage(int page, int pageSize, List<Tag> tagList)
        {
            try
            {
                _logger.LogInformation($"Start getting all tags from page {page} at {DateTime.UtcNow.ToLongTimeString()}");
                using HttpResponseMessage response = await _client.GetAsync($"https://api.stackexchange.com/2.3/tags?page={page}&pagesize={pageSize}&order=desc&site=stackoverflow");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var tags = JsonSerializer.Deserialize<Tags>(data);
                    foreach (var item in tags.Items)
                    {
                        if (item != null)
                        {
                            item.Percentage = Math.Round(GetPercentage(tags.Items, item), 2, MidpointRounding.AwayFromZero);
                            tagList.Add(item);
                        }
                    }
                }
                else
                {
                    _logger.LogError($"HttpResponse StatusCode: {response.StatusCode}");
                }
                _logger.LogInformation($"End getting all tags from page {page} at {DateTime.UtcNow.ToLongTimeString()}");
            }
            catch(Exception ex)
            {
                _logger.LogError($"TagsRepository AddTagsFromPage error: {ex}");
            }
        }
        public double GetPercentage(List<Tag> allTags, Tag tag)
        {
            try
            {
                double sumTagCount = allTags.Sum(t => t.Count);
                return (tag.Count / sumTagCount) * 100;
            }catch(Exception ex)
            {
                _logger.LogError($"TagsRepository Percentage error: {ex}");
                return 0.0;
            }
        }
        public List<Tag> GetTagsPerPage(List<Tag> allTags, int page, int pageSize)
        {
            return allTags.Skip((page - 1) * pageSize).Take(pageSize).ToList() ?? new List<Tag>();
        }
        public List<Tag> GetSortedTags(List<Tag> allTags, TagSortingColumn tagSortingColumn, TagSortingOrder tagSortingOrder)
        {
            try
            {
                if (tagSortingOrder == TagSortingOrder.desc)
                {
                    if (tagSortingColumn == TagSortingColumn.Name)
                    {
                        allTags = allTags.OrderByDescending(tag => tag.Name).ToList();
                    }
                    else
                    {
                        allTags = allTags.OrderByDescending(tag => tag.Count).ToList();
                    }
                }
                else
                {
                    if (tagSortingColumn == TagSortingColumn.Name)
                    {
                        allTags = allTags.OrderBy(tag => tag.Name).ToList();
                    }
                    else
                    {
                        allTags = allTags.OrderBy(tag => tag.Count).ToList();
                    }
                }
                return allTags;
            }
            catch(Exception ex)
            {
                _logger.LogError($"TagsRepository GetSortedTags error: {ex}");
                return new List<Tag>();
            }
        }
        public async Task SaveTagsToTheDatabase(List<Tag> tags)
        {
            await _context.Tag.AddRangeAsync(tags);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Tag>> GetTagsFromTheDatabase()
        {
            return await _context.Tag.ToListAsync();
        }
    }
}
