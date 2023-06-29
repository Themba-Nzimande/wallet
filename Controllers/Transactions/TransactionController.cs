namespace WebApplication1.Controllers.Transactions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Controllers.Transactions.DTOs;
    using WebApplication1.Repos.Accounts;

    /// <summary>
    /// API controller for transaction related enpoint for getting transactions related to account.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [RequireHttps]
    public class TransactionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IAccountsRepo _accountsRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="configuration">appsettings instance.</param>
        /// <param name="accountsRepo">accounts DB repo instance.</param>
        public TransactionController(IConfiguration configuration, IAccountsRepo accountsRepo)
        {
            this._configuration = configuration;
            this._accountsRepo = accountsRepo;
        }

        /// <summary>
        /// Returns list of transactions linked to user's account(s).
        /// </summary>
        /// <returns>List of transaction DTO objects.</returns>
        [HttpGet("TransactionHistory")]
        [Authorize]
        public IEnumerable<Account_transactionDTO> Get()
        {
            try
            {
                var result = new List<Account_transactionDTO>();

                var accountID = this._accountsRepo.GetUserAccountMappings().First(a => a.User_Id == int.Parse(this.User.Claims.FirstOrDefault(a => a.Type == "UserID").Value));

                var accountsForTransaction = this._accountsRepo.GetAccounts().Where(a => a.Id == accountID.Account_Id);

                foreach (var account in accountsForTransaction)
                {
                    var transactionsForAccount = this._accountsRepo.GetTransactions(account.Id.ToString());
                    foreach (var transaction in transactionsForAccount.OrderBy(a => a.Transaction_timestamp))
                    {
                        result.Add(new Account_transactionDTO()
                        {
                            Amount = transaction.Amount,
                            Transcation_entry_type = transaction.Transcation_entry_type == 1 ? "debit" : "credit",
                            Transaction_timestamp = transaction.Transaction_timestamp,
                        });
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
