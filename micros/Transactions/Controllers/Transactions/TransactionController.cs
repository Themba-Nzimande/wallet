namespace WebApplication1.Controllers.Transactions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Controllers.Account.DTOs;
    using WebApplication1.Controllers.Transactions.DTOs;
    using WebApplication1.Helpers;
    using WebApplication1.Models;
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

        /// <summary>
        /// Endpoint to create new account transaction for user's account.
        /// </summary>
        /// <param name="new_account_Transaction">new transaction object containing transaction details.</param>
        /// <returns>string stating if transaction was successful or not.</returns>
        [HttpPost("Transact")]
        [Authorize]
        public IActionResult Post(NewTransactionDTO new_account_Transaction)
        {
            try
            {
                var transactionsHelper = new TransactionHelper();

                var accountID = this._accountsRepo.GetUserAccountMappings().First(a => a.User_Id == int.Parse(this.User.Claims.FirstOrDefault(a => a.Type == "UserID").Value));

                var accountForTransaction = this._accountsRepo.GetAccounts().First(a => a.Id == accountID.Account_Id);
                var newTranscation = new Account_transaction()
                {
                    Transaction_timestamp = DateTime.Now,
                    Amount = new_account_Transaction.Amount,
                    Account_id = accountForTransaction.Id,
                    Transcation_entry_type = new_account_Transaction.Transcation_entry_type,
                };
                var transactionResult = transactionsHelper.TransactOnAccount(accountForTransaction, newTranscation);

                if (transactionResult)
                {
                    var transactionAdded = this._accountsRepo.CreateNewTransaction(newTranscation);
                    if (transactionAdded)
                    {
                        var accountBalanceUpdated = this._accountsRepo.UpdateAccount(accountForTransaction);
                        if (accountBalanceUpdated)
                        {
                            return this.Ok("Transaction succesful");
                        }
                        else
                        {
                            return this.Ok("Transaction processing failed. Please try again");
                        }
                    }
                    else
                    {
                        return this.Ok("Transaction processing failed. Please try again");
                    }
                }
                else
                {
                    return this.Ok("Transaction unsuccesful due to insufficient funds");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
