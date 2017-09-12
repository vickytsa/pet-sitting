using System.Configuration;
using System.Web.Mvc;
using VickyTsao.PetCare.Objects;
using VickyTsao.PetCare.WebApi.Client;

namespace VickyTsao.PetCare.WebSite.Controllers
{
    public class HomeController : Controller
    {
        string _baseURl = ConfigurationManager.AppSettings["WebServiceUrl"];


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllPetCategoryOptions()
        {
            PetCareWebApiClient client = new PetCareWebApiClient(_baseURl);

            var options = client.GetAllPetCategoryOptions();

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllPetSizeOptions()
        {
            PetCareWebApiClient client = new PetCareWebApiClient(_baseURl);

            var options = client.GetAllPetSizeOptions();

            return Json(options, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllPetCareOptionOptions()
        {
            PetCareWebApiClient client = new PetCareWebApiClient(_baseURl);

            var options = client.GetAllPetCareOptionOptions();

            return Json(options, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult FindPetSitters(PetSitterRequest request)
        {
            PetCareWebApiClient client = new PetCareWebApiClient(_baseURl);

            var sitters = client.FindPetSitters(request);

            return Json(sitters);
        }


        public ActionResult About()
        {
            ViewBag.Message = "We provide better care with lower affordable price for all pets and their owners!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Do you have any feedback?";

            return View();
        }
    }
}
