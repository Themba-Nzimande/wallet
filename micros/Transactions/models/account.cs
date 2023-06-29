namespace WebApplication1.Models
{

    /// <summary>
    /// Model for wallet application user account.
    /// </summary>
    public class Account
    {

        /// <summary>
        /// Gets or Sets unique (DB Pk) Id of account.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Gets or Sets account number of account.
        /// </summary>
        public int Account_number { get; set; }


        /// <summary>
        /// Gets or Sets current balance of account.
        /// </summary>
        public float Current_balance { get; set; }
    }
}
