using AddressBook.Areas.LOC_Cascade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AddressBook.Areas.LOC_Cascade.Controllers
{
    [Area("LOC_Cascade")]
    [Route("LOC_Cascade/[controller]/[action]")]
    public class LOC_CascadeController : Controller
    {
        public IActionResult Index()
        {
            Repository ro = new Repository();
            ViewBag.CountryList = new SelectList(ro.Countries, "Id", "Name");
            return View();
        }
        [HttpPost]
        public JsonResult GetState(string CountryId)
        {
            Repository ro = new Repository();
            var StateList = ro.States.Where(z => z.CountryId == Convert.ToInt32(CountryId));
            return Json(StateList);
            //return Json(new SelectList(StateList, "Id", "Name"));
        }
    }
}
