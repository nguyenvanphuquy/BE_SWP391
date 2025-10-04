using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;
        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<SubCategoryResponse>> GetAll()
        {
            var subCategory = _subCategoryService.GetAll();
            return Ok(subCategory);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var subCategory = _subCategoryService.GetById(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            return Ok(subCategory);
        }
        [HttpPost]
        public IActionResult Create([FromBody] SubCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subCategory = _subCategoryService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = subCategory.SubcategoryId }, subCategory);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SubCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var subCategory = _subCategoryService.Update(id, request);
            if (subCategory == null) return NotFound();
            return Ok(subCategory);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _subCategoryService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
