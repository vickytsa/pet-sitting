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
    public class PetCareOptionController : ApiController
    {
        private string _conn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;

        // GET: api/PetCareOption
        public IEnumerable<PetCareOption> Get()
        {
            var client = new SqlClient(_conn);

            return client.GetAllPetCareOptionOptions();
        }

        // GET: api/PetCareOption/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PetCareOption
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PetCareOption/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PetCareOption/5
        public void Delete(int id)
        {
        }
    }
}
