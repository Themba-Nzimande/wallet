namespace WebApplication1.Helpers
{
    using WebApplication1.Models;


    /// <summary>
    /// Transactions helper for functions that can be reused.
    /// </summary>
    public class TransactionHelper
    {
        /// <summary>
        /// Checks if a debit transaction can be completed with the account current balance.
        /// Also updates account current balance if transaction is possible.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="account_Transaction"></param>
        /// <returns>True if transaction can be done with current balance on account. False if account doesn't have funds to cover debit transaction.</returns>
        public bool TransactOnAccount(Account account, Account_transaction account_Transaction)
        {
            try
            {
                var result = false;

                // Check if account has enough funds if it's a debit transcation
                if (account_Transaction.Transcation_entry_type == 1 && 
                    account.Current_balance >= account_Transaction.Amount) 
                {
                    account.Current_balance = account.Current_balance - account_Transaction.Amount;
                }
                if (account_Transaction.Transcation_entry_type == 2)
                {
                    account.Current_balance = account.Current_balance + account_Transaction.Amount;
                    result = true;
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
