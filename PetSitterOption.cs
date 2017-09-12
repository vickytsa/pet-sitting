using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VickyTsao.PetCare.Objects;

namespace VickyTsao.PetCare.Objects
{
    public class PetSitterOption
    {
        [Required]
        public int SitterId { get; set; }

        [Required]
        public int PetCategoryId { get; set; }

        [Required]
        public string PetSizeId { get; set; }

        [Required]
        public int PetCareOptionId  { get; set; }

        [Required]
        public decimal Rate { get; set; }


    }
}
