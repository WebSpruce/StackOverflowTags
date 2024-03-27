using StackOverflowTags.Data;
using StackOverflowTags.Interfaces;
using StackOverflowTags.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace StackOverflowTags.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly HttpClient _client;
        public TagsRepository()
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            _client = new HttpClient(handler);
        }
        public async Task<List<Tag>> GetAllAsync()
        {
            try 
            { 
                List<Tag> tagList = new List<Tag>();
                for (int i = 1; i < 11; i++)
                {
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
                }
                return tagList;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"TagsRepository GetAllAsync error: {ex}");
                return new List<Tag>();
            }
        }
    }
}
