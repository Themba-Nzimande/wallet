namespace WebApplication1.Repos.Accounts
{
    using WebApplication1.Models;

    /// <summary>
    /// Interface for AccountsRepo.
    /// </summary>
    public interface IAccountsRepo
    {

        /// <summary>
        /// Resturns list of Account transcations objects. Can be filter to transactions related to an account ID.
        /// </summary>
        /// <param name="accountID">filter to transactions related to an account ID.</param>
        /// <returns>list of Account transcations objects.</returns>
        public List<Account_transaction> GetTransactions(string? accountID = null);

        /// <summary>
        /// Creates new account transaction DB entry.
        /// </summary>
        /// <param name="newTranscation">account transaction object.</param>
        /// <returns>True if transaction was succesfully inserted.</returns>
        public bool CreateNewTransaction(Account_transaction newTranscation);

        /// <summary>
        /// Gets list of account objects.
        /// </summary>
        /// <param name="mappingID">Used to filter list of accounts based off mapping.</param>
        /// <returns>list of account objects.</returns>
        public List<Account> GetAccounts(string? mappingID = null);

        /// <summary>
        /// Create new account entry in DB.
        /// </summary>
        /// <param name="newAccount">account object for creation.</param>
        /// <returns>True if account was succesfully inserted.</returns>
        public bool CreateNewAccount(Account newAccount);


        /// <summary>
        /// Updates account DB entry current balance field only.
        /// </summary>
        /// <param name="account">existing account object.</param>
        /// <returns>True if account current balance was succesfully updated.</returns>
        public bool UpdateAccount(Account account);

        /// <summary>
        /// Gets all user account mappings.
        /// </summary>
        /// <returns>List of user account mappings.</returns>
        public List<UserAccountMapping> GetUserAccountMappings();

        /// <summary>
        /// Creates new user account mapping.
        /// </summary>
        /// <param name="newUserAccountMapping">New user account mapping object for creation in DB.</param>
        /// <returns>True if user account mapping was succesfully created.</returns>
        public bool CreateNewUserAccountMapping(UserAccountMapping newUserAccountMapping);
    }
}
