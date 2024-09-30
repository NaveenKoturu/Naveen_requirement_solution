using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatientManagementWebApp.Models;

namespace PatientManagementWebApp.Controllers
{
    public class ManagePatientController : Controller
    {
        private readonly HttpClient _httpClient;

        public ManagePatientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7036/"); // Base API URL
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Patients");
            if (response.IsSuccessStatusCode)
            {
                var patientJson = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<Patient>>(patientJson);
                return View(patients);
            }
            return View(new List<Patient>()); 
        }

        public async Task<ActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/Patients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var patientJson = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(patientJson);
                return View(patient);
            }
            return NotFound();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Patient patient)
        {
            if (patient.DateOfBirth < new DateTime(1753, 1, 1))
            {
                ModelState.AddModelError("DateOfBirth", "Date of birth must be a valid date after 1/1/1753.");
                return View(patient);
            }

            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7036/api/Patients", patient);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Unable to create patient. API Error: {errorMessage}");

            return View(patient);
        }


        public async Task<ActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Patients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var patientJson = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(patientJson);
                return View(patient);
            }
            return NotFound(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient);
            }

            var response = await _httpClient.PutAsJsonAsync($"api/Patients/{id}", patient);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Unable to edit patient.");
            return View(patient); 
        }

        public async Task<ActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"api/Patients/{id}");
            if (response.IsSuccessStatusCode)
            {
                var patientJson = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(patientJson);
                return View(patient); 
            }
            return NotFound(); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int PatientID)
        {
            var response = await _httpClient.DeleteAsync($"api/Patients/{PatientID}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Unable to delete patient.");
            return RedirectToAction(nameof(Delete), new { PatientID }); 
        }
    }
}
