using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WalkersApp.Models.Domain;
using WalkersApp.Models.DTOs;

namespace WalkersApp.Mappings
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegionDTO, Region>().ReverseMap();
            CreateMap<AddRegionDTO, Region>();
            CreateMap<UpdateRegionDTO, Region>();
            CreateMap<UpdateWalkDTO, Walk>();
            CreateMap<AddWalksRequestDTO, Walk>();
            CreateMap<WalkDTO, Walk>().ReverseMap();
            CreateMap<DifficultyDTO, Difficulty>().ReverseMap();

            // maps region to region DTO
            //use reverse() to do the vice versa also.
            CreateMap<ExDomain, Example>()
            .ForMember(x => x.FullName,
            options => options.MapFrom(x => x.Name))
            .ReverseMap();
            // the prop names do not match use this function to map them explicitly.
        }

    }

    public class Example
    {
        public int FullName { get; set; }
    }
    public class ExDomain
    {
        public int Name { get; set; }
    }
}