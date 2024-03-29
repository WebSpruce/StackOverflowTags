using Microsoft.AspNetCore.Mvc;
using StackOverflowTags.Models;
using StackOverflowTags.Interfaces;
using StackOverflowTags.Data;
using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Helper;

namespace StackOverflowTags.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly ITagsRepository _tagsRepository;
        private readonly AppDbContext _context;
        public TagsController(ILogger<TagsController> logger, ITagsRepository tagsRepository, AppDbContext context)
        {
            _logger = logger;
            _tagsRepository = tagsRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Tags>> GetAll(int page = 1, int pageSize = 10, TagSortingColumn sortOption = TagSortingColumn.Name, TagSortingOrder sortOrder = TagSortingOrder.asc)
        {
            try
            {
                _logger.LogInformation($"Start getting all tags at {DateTime.UtcNow.ToLongTimeString()}");
                var tagList = await _tagsRepository.GetAllAsync();
                if (tagList.Any())
                {
                    _logger.LogInformation($"Resetting database values at {DateTime.UtcNow.ToLongTimeString()}");
                    await _context.Tag.ExecuteDeleteAsync();
                    await ResetAutoIncrement.Reset(_context);

                    await _context.Tag.AddRangeAsync(tagList);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"All operations have completed receiving tags at {DateTime.UtcNow.ToLongTimeString()}");

                    //pagination
                    var tagsPerPage = _tagsRepository.GetTagsPerPage(tagList, page, pageSize);
                    //sorting
                    var sortedTags = _tagsRepository.GetSortedTags(tagsPerPage, sortOption, sortOrder);

                    return Ok(sortedTags);
                }
                else
                {
                    _logger.LogError($"The list of tags doesn't contain any tags.");
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while getting all tags: {ex}");
                return NotFound();
            }
        }
    }
}
