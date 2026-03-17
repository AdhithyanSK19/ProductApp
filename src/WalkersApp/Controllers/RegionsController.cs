using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalkersApp.CustomActionFilters;
using WalkersApp.Data;
using WalkersApp.Models.Domain;
using WalkersApp.Models.DTOs;
using WalkersApp.Repository;

namespace WalkersApp.Controllers
{
    [ApiController] //this will automatically validate the modelstate and throws 400 error
    [Route("api/[controller]")]
    
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository,
            IMapper mapper)
        {
            //his.walksDbContext = walksDbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllAsync()
        {
            //var regions = await walksDbContext.Regions.ToListAsync();
            var regions = await regionRepository.GetAllAsync();

            // var regionsDtoList = new List<RegionDTO>();
            // foreach (Region region in regions)
            // {
            //     regionsDtoList.Add(new RegionDTO()
            //     {
            //         Id = region.Id,  
            //         Code = region.Code,
            //         Name = region.Name,
            //         RegionImageUrl = region.RegionImageUrl
            //     });
            // }
            //using Automapper
            var regionsDtoList = mapper.Map<List<RegionDTO>>(regions);
            return Ok(regionsDtoList);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        // this make it type safe. id here and in the func parameter should be same
        //if the id is not a guid, it will throw 404 error by itself.
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id) // good practice add FromRoute
        {
            //var region = await walksDbContext.Regions.FindAsync(id);
            var region = await regionRepository.GetByIddAsync(id); // works only for id
            //use firstOrDefault from Linq
            if (region == null)
                return NotFound();
            // var reggionDTO = new RegionDTO()
            // // to expose the model directly to client. Its not a good practice.
            // //in future if there is any change in the model, its affects the end client.
            // //its always better to use DTO
            // // We have decoupled the Domain Model from the view layer.
            // {
            //     Id = region.Id,
            //     Code = region.Code,
            //     Name = region.Name,
            //     RegionImageUrl = region.RegionImageUrl
            // };
            //now using Automapper.
            var reggionDTO = mapper.Map<RegionDTO>(region);
            return Ok(reggionDTO);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> AddRegionAsync([FromBody] AddRegionDTO addRegionDTO)
        {

            // var regionDomain = new Region()
            // {
            //     Code = addRegionDTO.Code,
            //     Name = addRegionDTO.Name,
            //     RegionImageUrl = addRegionDTO.RegionImageUrl
            // };
            //Using AutoMapper
            var regionDomain = mapper.Map<Region>(addRegionDTO);
            //await walksDbContext.Regions.AddAsync(regionDomain);
            //await walksDbContext.SaveChangesAsync();
            regionDomain = await regionRepository.CreateAsync(regionDomain);
            // var regionDTO = new RegionDTO()
            // {
            //     Id = regionDomain.Id,
            //     Code = regionDomain.Code,
            //     Name = regionDomain.Name,
            //     RegionImageUrl = regionDomain.RegionImageUrl
            // };
            var regionDTO = mapper.Map<RegionDTO>(regionDomain);
            //return Ok(regionDTO);
            //CreatedAtAction is needed for 201 response. name of fucntion, end route ID, and the value
            //return CreatedAtAction(nameof(GetByIdAsync), new { id = regionDTO.Id }, regionDTO);
            string uri = $"http://localhost:5048/api/Regions/{regionDTO.Id}";
            return Created(uri, regionDTO);
            //this sends 201 response.

            //in the response header of 201, we can find a header called location with the ID of the
            //created item. When the URL is pasted in the browser, it gives the same item.
            //this is the difference between 200 and 201
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {
            //Map DTO to Model
            // var regionDomain = new Region()
            // {
            //     Code = updateRegionDTO.Code,
            //     Name = updateRegionDTO.Name,
            //     RegionImageUrl = updateRegionDTO.RegionImageUrl
            // };
            //using mapper
            var regionDomain = mapper.Map<Region>(updateRegionDTO);
            //var regionDomain = await walksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);
            if (regionDomain == null)
                return NotFound();
            // regionDomain.Code = updateRegionDTO.Code;
            // regionDomain.Name = updateRegionDTO.Name;
            // regionDomain.RegionImageUrl = updateRegionDTO.RegionImageUrl;
            // // because this entity is already tracked, we do not need to add again.
            // //
            // await walksDbContext.SaveChangesAsync();

            // var regionDTO = new RegionDTO()
            // {
            //     Id = regionDomain.Id,
            //     Code = regionDomain.Code,
            //     Name = regionDomain.Name,
            //     RegionImageUrl = regionDomain.RegionImageUrl
            // };
            // //return Ok(regionDTO);

            var regionDTO = mapper.Map<Region>(regionDomain);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = regionDTO.Id }, regionDTO);


        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            //var regionDomain = await walksDbContext.Regions.FindAsync(id);
            var regionDomain = await regionRepository.DeleteAsync(id);
            if (regionDomain == null)
                return NotFound();
            // walksDbContext.Remove(regionDomain);
            // await walksDbContext.SaveChangesAsync();
            // //we can aslo return the deleted the region if needed but not mandate
            // var regionDTO = new RegionDTO()
            // {
            //     Id = regionDomain.Id,
            //     Code = regionDomain.Code,
            //     Name = regionDomain.Name,
            //     RegionImageUrl = regionDomain.RegionImageUrl
            // };
            var regionDTO = mapper.Map<RegionDTO>(regionDomain);
            return Ok(regionDTO);
        }
    }
}