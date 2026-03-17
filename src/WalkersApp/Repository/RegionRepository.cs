using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalkersApp.Data;
using WalkersApp.Models.Domain;

namespace WalkersApp.Repository
{
    public class RegionRespository : IRegionRepository
    {
        private readonly WalksDbContext walksDbContext;
        public RegionRespository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await walksDbContext.Regions.AddAsync(region);
            await walksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await walksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRegion == null)
                return null;
            walksDbContext.Regions.Remove(existingRegion);
            await walksDbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await walksDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIddAsync(Guid id)
        {
            return await walksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await walksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRegion == null)
                return null;
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await walksDbContext.SaveChangesAsync();
            return existingRegion;
        }

    }
}