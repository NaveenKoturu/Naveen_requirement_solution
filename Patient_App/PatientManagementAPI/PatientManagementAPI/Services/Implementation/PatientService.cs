using PatientManagementAPI.Models;
using PatientManagementAPI.Repositories.Implementation;
using PatientManagementAPI.Repositories.Interface;
using PatientManagementAPI.Services.Interfaces;

namespace PatientManagementAPI.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreatePatientAsync(Patient patient)
        {
            return await _repository.CreateAsync(patient);
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            return await _repository.UpdateAsync(patient);
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
