using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WalkersApp.Models.Domain;

namespace WalkersApp.Repository
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIddAsync(Guid id);
        //make the region return type nullable as we can
        //expect nullable values.

        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid id, Region region);
        //make the region return type nullable as we can
        //expect nullable values.

        Task<Region?> DeleteAsync(Guid id);
    }
}