using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VickyTsao.PetCare.Objects;
using VickyTsao.PetCare.Sql;

namespace VickyTsao.PetCare.WebApi.Controllers
{
    public class PetSitterBlockDateController : ApiController
    {
        private string _conn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;


        // PUT: api/PetSitterBlcokDate/5
        public PetSitterBlockDate Put([FromBody]PetSitterBlockDate date)
        {
            var client = new SqlClient(_conn);
            return client.AddPetSitterBlockDate(date);
        }

        // DELETE: api/PetSitterBlcokDate/5
        public void Delete(int id)
        {
        }
    }
}
