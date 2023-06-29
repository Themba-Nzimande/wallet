namespace WebApplication1.Controllers.Transactions.DTOs
{
    /// <summary>
    /// DTO for an existing transaction object.
    /// </summary>
    public class Account_transactionDTO
    {
        /// <summary>
        /// Gets or sets transaction type for a transaction.
        /// </summary>
        public string? Transcation_entry_type { get; set; }

        /// <summary>
        /// Gets or sets amount for a transaction.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// Gets or sets timestamp of a transaction.
        /// </summary>
        public DateTime Transaction_timestamp { get; set; }

    }
}
