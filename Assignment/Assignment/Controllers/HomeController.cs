using Assignment.Models;
using AssignmentBAL.Interfaces;
using AssignmentDAL.Model;
using AssignmentDAL.Models;
using AssignmentDAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPatientService _patientService;

        public HomeController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPatients()
        {
            string pageNumber = HttpContext.Session.GetString("pageNumber");
            string pageSize = HttpContext.Session.GetString("pageSize");
            string search = HttpContext.Session.GetString("search");
            if (pageNumber == null)
            {
                pageNumber = "1";
            }
            if (pageSize == null)
            {
                pageSize = "10";
            }

            PagedList<TableView> model = await _patientService.GetPatients(int.Parse(pageNumber), int.Parse(pageSize),search);
            return PartialView("PatientsTable", model);
        }

        public List<Doctor> GetDoctors()
        {
            List<Doctor> model = _patientService.GetDoctors();
            return model;
        }


        public IActionResult AddPatient(TableView model)
        {
            _patientService.AddPatient(model);
            return RedirectToAction("Index");
        }

        public JsonResult DeletePatient(int patientid)
        {
            if (_patientService.DeletePatient(patientid))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> EditPatient(int patientid)
        {
            List<TableView> patients = await _patientService.GetPatients();
            TableView model = patients.FirstOrDefault(x => x.Patientid == patientid);
            return PartialView("AddPatient", model);
        }

        public JsonResult ChangePage(int pageNumber)
        {
            if (pageNumber != 0)
            {
                HttpContext.Session.SetString("pageNumber", pageNumber.ToString());
            }
            return Json(new { success = true });
        }

        public JsonResult ChangePageSize(int pageSize)
        {
            if (pageSize != 0)
            {
                HttpContext.Session.SetString("pageSize", pageSize.ToString());
            }
            return Json(new { success = true });
        }

        public JsonResult SearchPatients(string search)
        {
            if(search != null)
            {
                HttpContext.Session.SetString("search", search.ToLower());
            }
            else
            {
                HttpContext.Session.SetString("search", "");
            }
            return Json(new { success = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}