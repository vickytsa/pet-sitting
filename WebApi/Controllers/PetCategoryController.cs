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
    public class PetCategoryController : ApiController
    {
        private string _conn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;
        
        // GET: api/PetCategory
        public IEnumerable<PetCategory> Get()
        {
            var client = new SqlClient(_conn);

            return client.GetAllPetCategoryOptions();
        }


        // GET: api/PetCategory/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PetCategory
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PetCategory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PetCategory/5
        public void Delete(int id)
        {
        }
    }
}
