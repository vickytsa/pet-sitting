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
    public class PetSizeController : ApiController
    {
        private string _conn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;

        // GET: api/PetSize
        public IEnumerable<PetSize> Get()
        {
            var client = new SqlClient(_conn);

            return client.GetAllPetSizeOptions();
        }

        // GET: api/PetSize/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PetSize
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PetSize/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PetSize/5
        public void Delete(int id)
        {
        }
    }
}
