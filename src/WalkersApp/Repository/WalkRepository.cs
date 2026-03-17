using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WalkersApp.Data;
using WalkersApp.Models.Domain;

namespace WalkersApp.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly WalksDbContext walksDbContext;
        public WalkRepository(WalksDbContext walksDbContext)
        {
            this.walksDbContext = walksDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await walksDbContext.Walks.AddAsync(walk);
            await walksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomain = await walksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomain == null)
                return null;
            walksDbContext.Walks.Remove(walkDomain);
            await walksDbContext.SaveChangesAsync();
            return walkDomain;

        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAsc = true, int pageNumber = 1, int pageSize = 1000)
        {
            // var walksList = await walksDbContext.Walks.Include("Difficulty")
            // .Include("Region").ToListAsync();

            var walks = walksDbContext.Walks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                //filtering
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                //sorting
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAsc ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
            }
            //pagination
            return await walks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = walksDbContext.Walks.Include("Difficulty").Include("Region")
            .FirstOrDefaultAsync(w => w.Id == id);
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkDomain = await walksDbContext.Walks
            .Include("Difficulty")
            .Include("Region")
            .FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomain == null)
                return null;
            walkDomain.Name = walk.Name;
            walkDomain.Description = walk.Description;
            walkDomain.LengthInKm = walk.LengthInKm;
            walkDomain.WalkImageUrl = walk.WalkImageUrl;
            walkDomain.DifficultyId = walk.DifficultyId;
            walkDomain.RegionId = walk.RegionId;
            await walksDbContext.SaveChangesAsync();
            return walkDomain;
        }
    }
}