using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkersApp.Models.Domain;
using WalkersApp.Models.DTOs;

namespace WalkersApp.Repository
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy=null,bool isAsc=true,int pageNumber =1,int pageSize=1000);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}