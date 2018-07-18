using Dapper;
using ITSSelfRunning.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITSSelfRunning.DataAccess
{
    public class RunnerRepository : IRunnerRepository
    {
        private readonly string _connectionString;
        public RunnerRepository(string cs)
        {
            _connectionString = cs;
        }

        public void Delete(int idRunner)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"DELETE FROM [dbo].[Runner]
                    WHERE id = @id";

                connection.Query(query, new { id = idRunner });
            }
        }

        public IEnumerable<Runner> Get()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [id]
                                  ,[Username]
                                  ,[LastName]
                                  ,[FirstName]
                                  ,[BirthDate]
                                  ,[Sex]
                                  ,[PhotoUri]
                              FROM [dbo].[Runner]";

                return connection.Query<Runner>(query).ToList();
            }
        }

        public async Task<Runner> GetRunner(string UserAspNet)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = $"SELECT * FROM [dbo].[Runner] where [dbo].Runner.Username = '{UserAspNet}'";


                return await connection.QueryFirstAsync<Runner>(query, UserAspNet);
            }
        }

        public Runner Get(int idRunner)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"SELECT [id]
                                  ,[Username]
                                  ,[LastName]
                                  ,[FirstName]
                                  ,[BirthDate]
                                  ,[Sex]
                                  ,[PhotoUri]
                              FROM [dbo].[Runner]
                              WHERE id = @id";

                return connection.QueryFirstOrDefault<Runner>(query, new { id = idRunner });
            }
        }

        public void Insert(Runner value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"
                        INSERT INTO [dbo].[Runner]
                                ([Username]
                                ,[LastName]
                                ,[FirstName]
                                ,[BirthDate]
                                ,[Sex]
                                ,[PhotoUri])
                         VALUES
                               (@Username
                               ,@LastName
                               ,@FirstName
                               ,@BirthDate
                               ,@Sex
                               ,@PhotoUri)";

                connection.Query(query, value);
            }

        }

        public void Update(Runner value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    @"UPDATE [dbo].[Runner]
                       SET [Username] = @Username
                          ,[LastName] = @LastName
                          ,[FirstName] = @FirstName
                          ,[BirthDate] = @BirthDate
                          ,[Sex] = @Sex
                          ,[PhotoUri] = @PhotoUri
                     WHERE id = @id";

                connection.Query(query, value);
            }
        }
    }
}
