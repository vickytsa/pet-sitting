using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickyTsao.PetCare.Objects
{
    public class PetReservation
    {
        [Required]
        //[JsonIgnore]
        public int ReservationId { get; set; }

        public string PetCareOptionName { get; set; }

        [Required]
        public int PetId { get; set; }

        public string PetName { get; set; }

        public decimal Rate { get; set; }

        public decimal SubTotal { get; set; }
    }
}
