using StackOverflowTags.Models;

namespace StackOverflowTags.Interfaces
{
    public interface ITagsRepository
    {
        Task<List<Tag>> GetAllAsync();
    }
}
