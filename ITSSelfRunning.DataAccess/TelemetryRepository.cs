using Dapper;
using ITSSelfRunning.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ITSSelfRunning.DataAccess
{
    public class TelemetryRepository
    {
        private readonly string _connectionString;
        public TelemetryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Telemetry> GetTelemetries(int IdActivity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT [IdTelemetry] ,[Latitude] ,[Longitude] ,[Moment] ,[UriSelfie] ,[Activity_Id]" +
                            "FROM[dbo].[Telemetry] WHERE Activity_Id = @id";

                return connection.Query<Telemetry>(query, new { id = IdActivity }).ToList();
            }
        }
    }
}
