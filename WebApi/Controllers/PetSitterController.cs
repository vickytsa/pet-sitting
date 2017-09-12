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
    public class PetSitterController : ApiController
    {
        private string _conn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConnectionString"]].ConnectionString;


        // GET: api/PetSitter
        public IEnumerable<PetSitter> Get()
        {
         
            var client = new SqlClient(_conn);

            return client.GetAllPetSitters();
        }

        // GET: api/PetSitter/5
        public PetSitter Get(int id)
        {
            
            var client = new SqlClient(_conn);

            return client.GetPetSitterById(id);
        }


        // POST: api/PetSitter
        public IEnumerable<PetSitter> Post([FromBody]PetSitterRequest request)
        {
            if (ModelState.IsValid)
            {
                
                var client = new SqlClient(_conn);

                return client.FindPetSitters(request);
            }
            else
            {
                var errorString = "";
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorString += error.Exception?.Message.ToString() + " ";
                    }
                    
                }
                throw new ArgumentException("Missing required parameters: " + errorString);
            }
               

        }

        // PUT: api/PetSitter/5
        public PetSitter Put([FromBody]PetSitter sitter)
        {
            
            var client = new SqlClient(_conn);

            return client.AddPetSitter(sitter);
        }

        // DELETE: api/PetSitter/5
        public void Delete(int id)
        {
            
            var client = new SqlClient(_conn);

            client.DeletePetSitter(id);
        }
    }
}
