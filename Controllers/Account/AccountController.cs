namespace WebApplication1.Controllers.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Controllers.Account.DTOs;
    using WebApplication1.Helpers;
    using WebApplication1.Models;
    using WebApplication1.Repos.Accounts;

    /// <summary>
    /// API controller for account related endpoints such as getting an account's balance
    /// and creating a transaction on an account.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [RequireHttps]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IAccountsRepo _accountsRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="configuration">appsettings instance.</param>
        /// <param name="accountsRepo"></param>
        public AccountController(IConfiguration configuration, IAccountsRepo accountsRepo)
        {
            this._configuration = configuration;
            this._accountsRepo = accountsRepo;
        }

        /// <summary>
        /// Returns user's account current balance.
        /// </summary>
        /// <returns>float of user's account current balance.</returns>
        [HttpGet("GetBalance")]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var accountID = this._accountsRepo.GetUserAccountMappings().First(a => a.User_Id == int.Parse(this.User.Claims.FirstOrDefault(a => a.Type == "UserID").Value));

                var accountForTransaction = this._accountsRepo.GetAccounts().First(a => a.Id == accountID.Account_Id);
                return this.Ok(accountForTransaction.Current_balance);
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
