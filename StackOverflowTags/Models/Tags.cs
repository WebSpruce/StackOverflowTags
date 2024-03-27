using System.Text.Json.Serialization;

namespace StackOverflowTags_t1.Models
{
    public class Tags
    {
        [JsonPropertyName("items")]
        public List<Tag> Items { get; set; }
    }
}
