using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using System.Text;

namespace HalloDocBAL.Services
{
    public class TokenService : ITokenService
    {
        IUserRepository _userRepository;

        public TokenService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public string GenerateToken(string userEmail)
        {
            var expireTime = DateTime.UtcNow.AddHours(1);
            var tokenData = $"{userEmail}|{expireTime}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenData));
        }

        public (string email, bool isValid) ValidateToken (string token)
        {
            try
            {
                
                var tokenData = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = tokenData.Split('|');
                if (parts.Length == 2)
                {
                    var email = parts[0];
                    var tok = _userRepository.GetToken(email);
                    if (tok == null)
                    {
                        return (email, false);
                    }
                    var expiration = DateTime.ParseExact(parts[1], "dd-MM-yyyy HH:mm:ss", null);
                    var isValid = expiration > DateTime.UtcNow;
                    return (email, isValid);
                }
            }
            catch
            {

            }
            return (null, false);
        }

        public string GetToken(string email)
        {
            return _userRepository.GetToken(email);
        }

        public bool StoreToken(string emial, string token)
        {
            return _userRepository.storeToken(emial, token);
        }
    }
}


