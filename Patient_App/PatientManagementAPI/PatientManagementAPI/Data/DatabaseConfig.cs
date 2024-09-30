using System.Data.SqlClient;

namespace PatientManagementAPI.Data
{
    public class DatabaseConfig
    {
        private readonly IConfiguration _configuration;

        public DatabaseConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

    }
}
