using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WalkersApp.CustomActionFilters;
using WalkersApp.Models.Domain;
using WalkersApp.Models.DTOs;
using WalkersApp.Repository;

namespace WalkersApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAsc, [FromQuery] int pageNumber=1, [FromQuery] int pageSize=1000) //max that our app can handle
        {
            var walksDomainList = await walkRepository.GetAllAsync(filterOn, filterQuery,sortBy, isAsc ?? true,pageNumber,pageSize);
            return Ok(mapper.Map<List<WalkDTO>>(walksDomainList));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
                return NotFound();
            var walkDto = mapper.Map<WalkDTO>(walkDomain);
            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel] // Custom Action filter
        public async Task<IActionResult> CreateAsync([FromBody] AddWalksRequestDTO addWalksRequestDTO)
        {
            var walksDomain = mapper.Map<Walk>(addWalksRequestDTO);
            walksDomain = await walkRepository.CreateAsync(walksDomain);
            var walkDto = mapper.Map<WalkDTO>(walksDomain);
            string url = $"http://localhost:5048/api/Walks/{walkDto.Id}";
            return Created(url, walkDto);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var walkDomain = mapper.Map<Walk>(updateWalkDTO);
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            if (walkDomain == null)
                return NotFound();
            var walkDto = mapper.Map<WalkDTO>(walkDomain);
            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
                return NotFound();
            var walkDto = mapper.Map<WalkDTO>(walkDomain);
            return Ok(walkDto);
        }

    }
}