using PatientManagementAPI.Data;
using PatientManagementAPI.Models;
using PatientManagementAPI.Repositories.Interface;
using System.Data.SqlClient;

namespace PatientManagementAPI.Repositories.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DatabaseConfig _dbConfig;

        public PatientRepository(DatabaseConfig dbConfig)
        {
            _dbConfig = dbConfig;
        }

        public async Task<int> CreateAsync(Patient patient)
        {
            using (var conn = _dbConfig.GetConnection())
            {
                var cmd = new SqlCommand("CreatePatient", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PatientID", patient.PatientID);
                cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object?)patient.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object?)patient.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MedicalHistory", (object?)patient.MedicalHistory ?? DBNull.Value);

                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = _dbConfig.GetConnection())
            {
                var cmd = new SqlCommand("DeletePatient", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PatientID", id);
                await conn.OpenAsync();
                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            var patients = new List<Patient>();
            using (var conn = _dbConfig.GetConnection())
            {
                var cmd = new SqlCommand("GetAllPatients", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        patients.Add(MapToPatient(reader));
                    }
                }
            }
            return patients;
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            using (var conn = _dbConfig.GetConnection())
            {
                var cmd = new SqlCommand("GetPatientById", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PatientID", id);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapToPatient(reader);
                    }
                }
            }
            return null;
        }

        public async Task<bool> UpdateAsync(Patient patient)
        {
            using (var conn = _dbConfig.GetConnection())
            {
                var cmd = new SqlCommand("UpdatePatient", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@PatientID", patient.PatientID);
                cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object?)patient.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object?)patient.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MedicalHistory", (object?)patient.MedicalHistory ?? DBNull.Value);

                await conn.OpenAsync();
                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        private Patient MapToPatient(SqlDataReader reader)
        {
            return new Patient
            {
                PatientID = Convert.ToInt32(reader["PatientID"]),
                FirstName = reader["FirstName"].ToString() ?? string.Empty,
                LastName = reader["LastName"].ToString() ?? string.Empty,
                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                Email = reader["Email"].ToString() ?? string.Empty,
                PhoneNumber = reader["PhoneNumber"] as string,
                Address = reader["Address"] as string,
                MedicalHistory = reader["MedicalHistory"] as string,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                UpdatedAt = reader["UpdatedAt"] as DateTime?
            };
        }
    }
}