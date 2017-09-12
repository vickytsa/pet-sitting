using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickyTsao.PetCare.Objects
{
    public class PetSitter
    {
        public int SitterId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public IEnumerable<PetSitterOption> PetSitterOptions { get; set; }

        //public IEnumerable<PetSitterReview> PetSitterReviews { get; set; }

        public decimal Rating { get; set; }

    }
}
