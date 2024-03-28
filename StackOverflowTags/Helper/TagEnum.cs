
using System.Text.Json.Serialization;

namespace StackOverflowTags.Helper
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TagSortingColumn
    {
        Name = 0,
        Count = 1
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TagSortingOrder
    {
        asc = 0,
        desc = 1
    }
}
