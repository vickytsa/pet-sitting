using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickyTsao.PetCare.Objects
{
    public class Pet
    {
        public int PetId { get; set; }

        //[JsonIgnore]
        [Required]
        public int PetOwnerId { get; set; }

        [Required]
        public string PetName { get; set; }

        public DateTime Bod { get; set; }

        [Required]
        public string GenderId { get; set; }

        [Required]
        public int PetCategoryId { get; set; }

        [Required]
        public string PetSizeId { get; set; }

        public string Breed { get; set; }

    }
}
