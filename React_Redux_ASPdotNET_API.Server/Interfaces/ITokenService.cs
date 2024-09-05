namespace React_Redux_ASPdotNET_API.Server.Interfaces
{
    /// <summary>
    /// Interface for the token service.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for a given email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A JWT token.</returns>
        Task<string> GenerateJwtToken(string email);
    }
}
