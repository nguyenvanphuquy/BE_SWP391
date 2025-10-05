using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BE_SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        private readonly IMetaDataService _metaDataService;
        public MetaDataController(IMetaDataService metaDataService)
        {
            _metaDataService = metaDataService;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var metadata = _metaDataService.GetById(id);
            if (metadata == null) return NotFound();
            return Ok(metadata);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var metadatas = _metaDataService.GetAll();
            return Ok(metadatas);
        }
        [HttpPost]
        public IActionResult Create(MetaDataRequest request)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var metadata = _metaDataService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = metadata.MetadataId }, metadata);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, MetaDataRequest request)
        {

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            var metadata = _metaDataService.Update(id, request);
            return Ok(metadata);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _metaDataService.Delete(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
