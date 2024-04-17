namespace HalloDocBAL.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userEmail);
        (string email, bool isValid) ValidateToken(string token);
        string GetToken(string email);
        bool StoreToken(string email, string token);
    }
}
