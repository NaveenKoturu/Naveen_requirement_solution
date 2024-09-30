using Microsoft.AspNetCore.Mvc;
using PatientManagementAPI.Models;
using PatientManagementAPI.Services.Implementation;
using PatientManagementAPI.Services.Interfaces;


namespace PatientManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var patients = await _service.GetAllPatientsAsync();
            return Ok(patients);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _service.GetPatientByIdAsync(id);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Patient patient)
        {
            if (patient.DateOfBirth > DateTime.Now)
            {
                return BadRequest("Date of Birth cannot be in the future.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPatientId = await _service.CreatePatientAsync(patient);
            return CreatedAtAction(nameof(Get), new { id = createdPatientId }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Patient patient)
        {
            if (id != patient.PatientID)
                return BadRequest("ID mismatch");

            var isUpdated = await _service.UpdatePatientAsync(patient);
            if (!isUpdated)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeletePatientAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }

    }
}
