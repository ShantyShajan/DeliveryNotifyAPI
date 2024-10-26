using System.Data;
using Microsoft.Data.SqlClient;

namespace DeliveryNofityAPI.Utilities
{
    public class SqlConnections
    {
        private readonly string _connectionString;

        public SqlConnections(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
            
    }
}
