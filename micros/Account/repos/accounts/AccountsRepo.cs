namespace WebApplication1.Repos.Accounts
{
    using Npgsql;
    using WebApplication1.Models;

    /// <summary>
    /// Repo that connects to the DB and executes CRUD operations related to accounts.
    /// Includes Accounts, user account mappings and transactions.
    /// </summary>
    public class AccountsRepo : IAccountsRepo
    {
        private readonly IConfiguration _configuration;

        private string _connString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsRepo"/> class.
        /// Default consructor.
        /// </summary>
        /// <param name="configuration">appsettings instane for use to get db conn.</param>
        public AccountsRepo(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._connString = this._configuration.GetConnectionString("PostgresSqlDb");
        }

        #region Transactions
        /// <summary>
        /// Resturns list of Account transcations objects. Can be filter to transactions related to an account ID.
        /// </summary>
        /// <param name="accountID">filter to transactions related to an account ID.</param>
        /// <returns>list of Account transcations objects.</returns>
        public List<Account_transaction> GetTransactions(string? accountID = null)
        {
            try
            {
                var result = new List<Account_transaction>();

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    string slqSelectStatement = accountID != null ? "SELECT * FROM public.\"accounts_transactions\" WHERE account_ID = @account_id" : "SELECT * FROM public.\"accounts_transactions\"";
                    using (var command = new NpgsqlCommand(slqSelectStatement, connection))
                    {
                        if (accountID != null)
                        {
                            command.Parameters.AddWithValue("@account_id", int.Parse(accountID));
                        }

                        // Open the database connection and execute the SQL command
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            // Read data from the data reader
                            while (reader.Read())
                            {
                                result.Add(new Account_transaction
                                {
                                    Id = reader.GetInt16(0),
                                    Amount = float.Parse(reader.GetValue(1).ToString()),
                                    Account_id = reader.GetInt16(2),
                                    Transaction_timestamp = DateTime.Parse(reader.GetValue(3).ToString()),
                                    Transcation_entry_type = reader.ToString() == "debit" ? 1 : 2,
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
        /// Creates new account transaction DB entry.
        /// </summary>
        /// <param name="newTranscation">account transaction object.</param>
        /// <returns>True if transaction was succesfully inserted.</returns>
        public bool CreateNewTransaction(Account_transaction newTranscation)
        {
            try
            {
                var result = false;

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO public.\"accounts_transactions\" (amount, account_id, transaction_timestamp, transcation_entry_type) VALUES (@amount, @account_id, @transaction_timestamp, @transcation_entry_type)", connection))
                    {
                        command.Parameters.AddWithValue("@transcation_entry_type", newTranscation.Transcation_entry_type == 2 ? "credit" : "debit");
                        command.Parameters.AddWithValue("@amount", newTranscation.Amount);
                        command.Parameters.AddWithValue("@transaction_timestamp", DateTime.Now);
                        command.Parameters.AddWithValue("@account_id", newTranscation.Account_id);

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
        #endregion

        /// <summary>
        /// Gets list of account objects.
        /// </summary>
        /// <param name="mappingID">Used to filter list of accounts based off mapping.</param>
        /// <returns>list of account objects.</returns>
        public List<Account> GetAccounts(string? mappingID = null)
        {
            try
            {
                var result = new List<Account>();

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    string slqSelectStatement = mappingID != null ? "SELECT * FROM public.\"accounts\" WHERE accountID = @accountID" : "SELECT * FROM public.\"accounts\"";
                    using (var command = new NpgsqlCommand(slqSelectStatement, connection))
                    {
                        if (mappingID != null)
                        {
                            command.Parameters.AddWithValue("@accountID", mappingID);
                        }

                        // Open the database connection and execute the SQL command
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            // Read data from the data reader
                            while (reader.Read())
                            {
                                result.Add(new Account
                                {
                                    Id = reader.GetInt16(0),
                                    Account_number = int.Parse(reader.GetValue(1).ToString()),
                                    Current_balance = float.Parse(reader.GetValue(2).ToString()),
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
        /// Create new account entry in DB.
        /// </summary>
        /// <param name="newAccount">account object for creation.</param>
        /// <returns>True if account was succesfully inserted.</returns>
        public bool CreateNewAccount(Account newAccount)
        {
            try
            {
                var result = false;

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO public.\"accounts\" (account_number, current_balance) VALUES (@account_number, @current_balance)", connection))
                    {
                        command.Parameters.AddWithValue("@account_number", newAccount.Account_number);
                        command.Parameters.AddWithValue("@current_balance", newAccount.Current_balance);

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

        /// <summary>
        /// Updates account DB entry current balance field only.
        /// </summary>
        /// <param name="account">existing account object.</param>
        /// <returns>True if account current balance was succesfully updated.</returns>
        public bool UpdateAccount(Account account)
        {
            try
            {
                var result = false;

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    using (var command = new NpgsqlCommand("UPDATE public.\"accounts\" SET current_balance = @current_balance WHERE id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", account.Id);
                        command.Parameters.AddWithValue("@current_balance", account.Current_balance);

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

        #region AccountUserMappings
        /// <summary>
        /// Gets all user account mappings.
        /// </summary>
        /// <returns>List of user account mappings.</returns>
        public List<UserAccountMapping> GetUserAccountMappings()
        {
            try
            {
                var result = new List<UserAccountMapping>();

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    string slqSelectStatement = "SELECT * FROM public.\"user_accounts\"";
                    using (var command = new NpgsqlCommand(slqSelectStatement, connection))
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            // Read data from the data reader
                            while (reader.Read())
                            {
                                result.Add(new UserAccountMapping
                                {
                                    Id = reader.GetInt16(0),
                                    User_Id = reader.GetInt16(1),
                                    Account_Id = reader.GetInt16(2),
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
        /// Creates new user account mapping.
        /// </summary>
        /// <param name="newUserAccountMapping">New user account mapping object for creation in DB.</param>
        /// <returns>True if user account mapping was succesfully created.</returns>
        public bool CreateNewUserAccountMapping(UserAccountMapping newUserAccountMapping)
        {
            try
            {
                var result = false;

                using (var connection = new NpgsqlConnection(this._connString))
                {
                    using (var command = new NpgsqlCommand("INSERT INTO public.\"user_accounts\" ( user_id, account_id) VALUES ( @user_id, @account_id)", connection))
                    {
                        command.Parameters.AddWithValue("@user_id", newUserAccountMapping.User_Id);
                        command.Parameters.AddWithValue("@account_id", newUserAccountMapping.Account_Id);

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
        #endregion

    }
}
