using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ITSSelfRunning.Models;

namespace ITSSelfRunning.DataAccess
{
    public class ActivityRepository
    {
        private readonly string _connectionString;

        public ActivityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Activity Get(int IdActivity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = $"SELECT [IdActivity] ,[ActivityName] ,[Status] ,[CreationDate] ,[Location] ,[ActivityType] ,[UriGara]" + 
                            ",[Runner_Id] FROM[dbo].[Activity] WHERE IdActivity = @id";

                return connection.QueryFirstOrDefault<Activity>(query, new { id = IdActivity });
            }
        }

        public async Task InsertActivity(Activity act)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO [dbo].[Activity]" +
                            "([ActivityName],[Runner_Id] ,[CreationDate],[Location],[ActivityType] ,[UriGara], [Status])  " +
                            "VALUES " +
                            $"(@ActivityName," +
                            $"@Runner_id, DATEADD(HOUR, 2, GETDATE())," +
                            $"@Location, @ActivityType, @UriGara, @Status)";
                
                await connection.QueryAsync<Activity>(query, act);
            }
        }

        public async Task<IEnumerable<Activity>> GetTrainings(int IdRunner)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = $"select * from [dbo].[Activity] where Runner_Id = @id";

                return (await connection.QueryAsync<Activity>(query, new { id = IdRunner })).ToList();
            }
        }

        public void StartTraining(int IdActivity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = $"UPDATE [dbo].[Activity] SET[Status] = 1 WHERE IdActivity = @id";

                connection.Query(query, new { id = IdActivity });
            }
        }

        public void EndTraining(int IdActivity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = $"UPDATE [dbo].[Activity] SET[Status] = 2 WHERE IdActivity = @id"; //TODO: query update con set a 2

                connection.Query(query, new { id = IdActivity });
            }
        }


    }
}
