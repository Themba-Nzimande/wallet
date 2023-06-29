namespace WebApplication1.Repos.Users
{
    using WebApplication1.Models;

    /// <summary>
    /// Interface for UsersRepo.
    /// </summary>
    public interface IUserRepo
    {

        /// <summary>
        /// Gets list of User objects.
        /// </summary>
        /// <param name="userEmail">If not null then filters users to return one user in the list
        /// that matches the email.</param>
        /// <returns>list of User model objects.</returns>
        public List<User> GetUsers(string? userEmail = null);

        /// <summary>
        /// Inserts new user entry into  DB.
        /// </summary>
        /// <param name="newUser">new user object for insertion.</param>
        /// <returns>returns true if insertion was succesful.</returns>
        public bool CreateUser(User newUser);
    }
}
