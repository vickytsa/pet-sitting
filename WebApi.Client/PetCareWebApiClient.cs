using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VickyTsao.PetCare.Objects;

namespace VickyTsao.PetCare.WebApi.Client
{
    public class PetCareWebApiClient
    {
        private string _baseUrl;


        //Constructor
        public PetCareWebApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }


        public async Task<IEnumerable<PetCategory>> GetAllPetCategoryOptions()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appilcation/json"));

                IEnumerable<PetCategory> options = null;
                var response = await httpClient.GetAsync("api/PetCategory").ConfigureAwait(false); 

                if (response.IsSuccessStatusCode)
                {
                    options = await response.Content.ReadAsAsync<IEnumerable<PetCategory>>();
                }
                return options;
            }
        }

        public async Task<IEnumerable<PetSize>> GetAllPetSizeOptions()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appilcation/json"));

                IEnumerable<PetSize> options = null;
                var response = await httpClient.GetAsync("api/PetSize").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    options = await response.Content.ReadAsAsync<IEnumerable<PetSize>>();
                }
                return options;
            }
        }

        public async Task<IEnumerable<PetCareOption>> GetAllPetCareOptionOptions()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appilcation/json"));

                IEnumerable<PetCareOption> options = null;
                var response = await httpClient.GetAsync("api/PetCareOption").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    options = await response.Content.ReadAsAsync<IEnumerable<PetCareOption>>();
                }
                return options;
            }
        }

        public async Task<IEnumerable<PetSitter>> GetAllPetSitters()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appilcation/json"));

                IEnumerable<PetSitter> sitters = null;
                var response = await httpClient.GetAsync("api/PetSitter").ConfigureAwait(false); 

                if (response.IsSuccessStatusCode)
                {
                    sitters = await response.Content.ReadAsAsync<IEnumerable<PetSitter>>();
                }
                return sitters;
            } 
        }

        public async Task<IEnumerable<PetSitter>> FindPetSitters(PetSitterRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(_baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appilcation/json"));

                List<PetSitter> sitters = null;
                var response = await httpClient.PostAsJsonAsync("api/PetSitter",request).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    sitters = await response.Content.ReadAsAsync<List<PetSitter>>();
                }
                return sitters;
            }

        }

    }
}
