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
    public class PetSitterOptionController : ApiController
    {
        private string _conn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;


        // GET: api/PetSitterOption
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PetSitterOption/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PetSitterOption
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PetSitterOption/5
        public PetSitterOption Put([FromBody]PetSitterOption option)
        {
            
            var client = new SqlClient(_conn);

            return client.AddNewPetSitterOption(option);
        }

        // DELETE: api/PetSitterOption/5
        public void Delete([FromBody]PetSitterOption option)
        {
            
            var client = new SqlClient(_conn);

            client.DeletePetSitterOption(option);

        }
    }
}
