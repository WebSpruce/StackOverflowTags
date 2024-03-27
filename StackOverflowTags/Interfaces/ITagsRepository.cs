using StackOverflowTags_t1.Models;

namespace StackOverflowTags_t1.Interfaces
{
    public interface ITagsRepository
    {
        Task<List<Tag>> GetAllAsync();
    }
}
