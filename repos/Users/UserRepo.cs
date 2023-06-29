namespace WebApplication1.Repos.Users
{
    using Microsoft.Extensions.Configuration;
    using Npgsql;
    using WebApplication1.Models;

    /// <summary>
    /// Repo that connects to the DB and executes CRUD operations related to users.
    /// Includes getting and inserting user entries.
    /// </summary>
    public class UserRepo : IUserRepo
    {
        private readonly IConfiguration _configuration;

        private string _connString;

        ///
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepo"/> class.
        /// </summary>
        /// <param name="configuration">appsettings instane for use to get db conn.</param>
        public UserRepo(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connString = this._configuration.GetConnectionString("PostgresSqlDb");
        }

        /// <summary>
        /// Gets list of User objects.
        /// </summary>
        /// <param name="userEmail">If not null then filters users to return one user in the list
        /// that matches the email.</param>
        /// <returns>list of User model objects.</returns>
        public List<User> GetUsers(string? userEmail = null)
        {
            try
            {
                var result = new List<User>();

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    string slqSelectStatement = userEmail != null ? "SELECT * FROM public.\"users\" WHERE email = @email" : "SELECT * FROM public.\"users\"";
                    using (var command = new NpgsqlCommand(slqSelectStatement, connection))
                    {
                        if (userEmail != null)
                        {
                            command.Parameters.AddWithValue("@email", userEmail);
                        }

                        // Open the database connection and execute the SQL command
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            // Read data from the data reader
                            while (reader.Read())
                            {
                                result.Add(new User
                                {
                                    Id = reader.GetInt16(0),
                                    Email = reader.GetString(1),
                                    User_password = reader.GetValue(2).ToString(),
                                });
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts new user entry into  DB.
        /// </summary>
        /// <param name="newUser">new user object for insertion.</param>
        /// <returns>returns true if insertion was succesful.</returns>
        public bool CreateUser(User newUser)
        {
            try
            {
                var result = false;

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO public.\"users\" (email, user_password) VALUES (@email, @user_password)", connection))
                    {
                        command.Parameters.AddWithValue("@email", newUser.Email);
                        command.Parameters.AddWithValue("@user_password", newUser.User_password);

                        // Open the database connection and execute the SQL command
                        connection.Open();
                        command.ExecuteNonQuery();
                        result = true;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
