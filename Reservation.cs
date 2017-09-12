using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VickyTsao.PetCare.Objects;

namespace VickyTsao.PetCare.Objects
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required]
        public int SitterId { get; set; }

        public string PetSitterName { get; set; }

        [Required]
        public int PetOwnerId { get; set; }

        public string PetOwnerName { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime DropOffDate { get; set; }

        [Required]
        public DateTime PickUpDate { get; set; }

        //[JsonIgnore]
        [Required]
        public int PetCareOptionId { get; set; }

        public IEnumerable<PetReservation> PetReservations { get; set; }

        public decimal Total { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReservationStatus Status { get; set; }


        //public int PetId { get; set; }

        //public string PetName { get; set; }

        //public string PetCareOptionName { get; set; }

        //public decimal Rate { get; set; }

        //public decimal SubTotal { get; set; }



    }
}
