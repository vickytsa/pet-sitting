using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VickyTsao.PetCare.Objects
{
    public class ReservationRequest
    {
        public int? ReservationId { get; set; }

        public int? SitterId { get; set; }

        public int? PetOwnerId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

    }
}
