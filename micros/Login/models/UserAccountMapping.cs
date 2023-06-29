namespace WebApplication1.Models
{
    /// <summary>
    /// Model for wallet application user to account mapping. Which account Id belongs to which user Id.
    /// </summary>
    public class UserAccountMapping
    {
        /// <summary>
        /// Gets or Sets unique (DB Pk) Id of user account mapping.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets unique (DB FK) Id of user.
        /// </summary>
        public int User_Id { get; set; }

        /// <summary>
        /// Gets or Sets unique (DB FK) Id of account.
        /// </summary>
        public int Account_Id { get; set; }
    }
}
