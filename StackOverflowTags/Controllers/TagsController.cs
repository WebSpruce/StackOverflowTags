using Microsoft.AspNetCore.Mvc;
using StackOverflowTags.Models;
using System.Diagnostics;
using StackOverflowTags.Interfaces;
using StackOverflowTags.Data;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<Tags>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Start getting all tags at {DateTime.UtcNow.ToLongTimeString()}");
                var tagList = await _tagsRepository.GetAllAsync();
                if (tagList.Any())
                {
                    _logger.LogInformation($"Resetting database values at {DateTime.UtcNow.ToLongTimeString()}");
                    await _context.Tag.ExecuteDeleteAsync();

                    var connection = _context.Database.GetDbConnection();
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM sqlite_sequence WHERE name='Tag'";
                        await command.ExecuteNonQueryAsync();
                    }
                    foreach (var tag in tagList)
                    {
                        await _context.Tag.AddAsync(tag);
                    }
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"All operations have completed receiving tags at {DateTime.UtcNow.ToLongTimeString()}");

                    return Ok(tagList);
                }
                else
                {
                    _logger.LogError($"The list of tags doesn't contain any tags.");
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"TagsController GetAll error: {ex}");
                _logger.LogError($"Error while getting all tags.");
                return NotFound();
            }
        }
    }
}
