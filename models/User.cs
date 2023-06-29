namespace WebApplication1.Models
{

    /// <summary>
    /// Model for wallet application user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or Sets unique (DB Pk) Id of user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets email of user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or Sets encryted password of user.
        /// </summary>
        public string? User_password { get; set; }
    }
}
