namespace WebApplication1.Controllers.Account.DTOs
{

    /// <summary>
    /// DTO for a new transaction object.
    /// </summary>
    public class NewTransactionDTO
    {
        /// <summary>
        /// Gets or sets amount for a transaction.
        /// </summary>
        public float Amount { get; set; }


        /// <summary>
        /// Gets or sets transaction type for a transaction.
        /// </summary>
        public int Transcation_entry_type { get; set; }
    }
}
