using System.Text.Json.Serialization;

namespace StackOverflowTags.Models
{
    public class Tags
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("items")]
        public List<Tag> Items { get; set; }
    }
}
