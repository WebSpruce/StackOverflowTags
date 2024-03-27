﻿using System.Text.Json.Serialization;

namespace StackOverflowTags_t1.Models
{
    public class Tag
    {
        [JsonPropertyName("has_synonyms")]
        public bool Has_Synonyms { get; set; }
        [JsonPropertyName("is_moderator_only")]
        public bool Is_Moderator_Only { get; set; }
        [JsonPropertyName("is_required")]
        public bool Is_Required { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("last_activity_date")]
        public DateTime? Last_Activity_Date { get; set; }
        [JsonPropertyName("synonyms")]
        public List<string> Synonyms { get; set; }
        [JsonPropertyName("user_id")]
        public int? User_Id { get; set; }
    }
}
