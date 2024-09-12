namespace React_Redux_ASPdotNET_API.Server.Interfaces
{
    /// <summary>
    /// Interface for the token service.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for a given email, along with a refresh token.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A JWT token and a refresh token.</returns>
        Task<(string jwtToken, string refreshToken)> GenerateJwtTokenAsync(string email);

        /// <summary>
        /// Confirm if a refresh token is valid.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="refreshToken"></param>
        Task<bool> ValidateRefreshTokenAsync(string email, string refreshToken);

        /// <summary>
        /// Get claims principal from expired token
        /// </summary>
        /// <param name="token">Expired token</param>
        /// <returns>Claims principal</returns>
        Task<string> GetEmailFromExpiredTokenAsync(string token);
    }
}
