using StackOverflowTags.Helper;
using StackOverflowTags.Models;

namespace StackOverflowTags.Interfaces
{
    public interface ITagsRepository
    {
        Task<List<Tag>> GetAllAsync(int amountOfTags);
        double GetPercentage(List<Tag> allTags, Tag tag);
        List<Tag> GetTagsPerPage(List<Tag> allTags, int page, int pageSize);
        List<Tag> GetSortedTags(List<Tag> allTags, TagSortingColumn tagSortingColumn, TagSortingOrder tagSortingOrder);
    }
}
