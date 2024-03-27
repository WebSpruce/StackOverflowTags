using Microsoft.AspNetCore.Mvc;
using StackOverflowTags_t1.Models;
using System.Diagnostics;
using StackOverflowTags_t1.Interfaces;

namespace StackOverflowTags_t1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsRepository _tagsRepository;
        public TagsController(ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Tags>> GetAll()
        {
            try
            {
                var tagList = await _tagsRepository.GetAllAsync();
                if (tagList.Count > 0)
                {
                    return Ok(tagList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"TagsController GetAll error: {ex}");
                return NotFound();
            }
        }
    }
}
