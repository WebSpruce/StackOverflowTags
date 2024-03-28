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
        public TagsRepository(ILogger<TagsRepository> logger)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            _client = new HttpClient(handler);
            _logger = logger;

        }
        public async Task<List<Tag>> GetAllAsync()
        {
            try 
            { 
                List<Tag> tagList = new List<Tag>();
                for (int i = 1; i < 11; i++)
                {
                    _logger.LogInformation($"Start getting all tags from page {i} at {DateTime.UtcNow.ToLongTimeString()}");
                    using HttpResponseMessage response = await _client.GetAsync($"https://api.stackexchange.com/2.3/tags?page={i}&pagesize=100&order=desc&site=stackoverflow");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var json = JsonSerializer.Deserialize<Tags>(data);
                        foreach (var item in json.Items)
                        {
                            if (item != null)
                            {
                                tagList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError($"HttpResponse StatusCode: {response.StatusCode}");
                    }
                    _logger.LogInformation($"End getting all tags from page {i} at {DateTime.UtcNow.ToLongTimeString()}");
                }
                return tagList;
            }
            catch(Exception ex)
            {
                _logger.LogError($"TagsRepository GetAllAsync error: {ex}");
                return new List<Tag>();
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
    }
}
