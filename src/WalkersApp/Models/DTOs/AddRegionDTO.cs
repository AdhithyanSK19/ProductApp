using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalkersApp.Models.DTOs
{
    public class AddRegionDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code should be min len 3")]
        [MaxLength(3, ErrorMessage = "Code should be max len 3")]
        //data annotations to validate the model
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}