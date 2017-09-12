using System;
using System.ComponentModel.DataAnnotations;

namespace VickyTsao.PetCare.Objects
{
    public class PetSitterRequest
    {
        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public decimal? MinRate { get; set; }

        public decimal? MaxRate { get; set; }

        [Required]
        public string PetSizeId { get; set; }

        [Required]
        public int PetCategoryId { get; set; }
   
        [Required]
        public int PetCareOptionId { get; set; }

        public decimal? MinRating { get; set; }

        [Required]
        public DateTime DropOffDate { get; set; }

        [Required]
        public DateTime PickUpDate { get; set; }


    }
}
