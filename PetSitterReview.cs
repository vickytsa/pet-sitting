using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickyTsao.PetCare.Objects
{
    public class PetSitterReview
    {
        public int ReviewId { get; set; }

        [Required]
        public int SitterId { get; set; }

        [Required]
        public int PetOwnerId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

    }
}
