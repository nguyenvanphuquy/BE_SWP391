using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using Microsoft.AspNetCore.Authorization;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = categoryService.GetAll();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = categoryService.GetById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = categoryService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryRequest request)
        {
            var category = categoryService.Update(id, request);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = categoryService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
