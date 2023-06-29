namespace WebApplication1.Models
{

    /// <summary>
    /// Model for wallet application transcation.
    /// </summary>
    public class Account_transaction
    {

        /// <summary>
        /// Gets or Sets unique (DB Pk) Id of account transcation.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Gets or Sets transaction entry type (1 = dedit (minus) and 2 = credit (plus)).
        /// </summary>
        public int Transcation_entry_type { get; set; }


        /// <summary>
        /// Gets or Sets transcation amount.
        /// </summary>
        public float Amount { get; set; }


        /// <summary>
        /// Gets or Sets timestamp of transaction in full datetime excluding timezone.
        /// </summary>
        public DateTime Transaction_timestamp { get; set; }

        /// <summary>
        /// Gets or Sets unique (DB Pk) Id of account transaction belongs to.
        /// </summary>
        public int Account_id { get; set; }
    }
}
