using StackOverflowTags.Models;

namespace StackOverflowTags.Interfaces
{
    public interface ITagsRepository
    {
        Task<List<Tag>> GetAllAsync();
        double GetPercentage(List<Tag> allTags, Tag tag);
    }
}
