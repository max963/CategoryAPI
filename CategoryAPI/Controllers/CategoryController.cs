using CategoryAPI.Data;
using CategoryAPI.DTOs;
using CategoryAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ApiContext _context;

        public CategoryController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryModel>> Get()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryModel> Get(Guid id)
        {
            var category = _context.Categories.Find(id);
            return Ok(category);
        }

        [HttpPost]
        public ActionResult<CategoryModel> Post([FromBody] CategoryCreatingDTO dto)
        {
            var category = from c in _context.Categories
                           where c.Name == dto.Name
                           select c;

            if (category.FirstOrDefault() != null)
            {
                return BadRequest("Categoria já existente");
            }

            CategoryModel model = new CategoryModel();  
            model.Id = Guid.NewGuid();
            model.Name = dto.Name;
            _context.Categories.Add(model);
            _context.SaveChanges();

            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] CategoryModel model)
        {
            var category = _context.Categories.Find(model.Id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }

    }
}

