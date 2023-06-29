namespace WebApplication1.Controllers.Login.DTOs
{
    /// <summary>
    /// DTO for new user object.
    /// </summary>
    public class NewUserDTO
    {
        /// <summary>
        /// Gets or sets email for a new user account object.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets password for a new user account object.
        /// </summary>
        public string? Password { get; set; }
    }
}
