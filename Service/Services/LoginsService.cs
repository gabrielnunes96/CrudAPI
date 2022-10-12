using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Services.User;
using Domain.Repository;
using Domain.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;
        private IConfiguration _configuration { get; }
        private SigningConfigurations _signingConfigurations;
        private TokenConfiguration _tokenConfiguration;
        public LoginService(IUserRepository repository,
                            IConfiguration configuration,
                            TokenConfiguration tokenConfiguration,
                            SigningConfigurations signingConfigurations)
        {
            _repository = repository;
            _configuration = configuration;
            _tokenConfiguration = tokenConfiguration;
            _signingConfigurations = signingConfigurations;
        }

        public async Task<object> FindByLogin(LoginDTO userDTO)
        {
            var baseUser = new UserEntity();

            if (userDTO != null && !string.IsNullOrWhiteSpace(userDTO.Email))
            {
                baseUser = await _repository.FindByLogin(userDTO.Email);
                if (baseUser is null)
                {
                    return new
                    {
                        authenticated = false,
                        message = "Falha ao autenticar"
                    };
                }
                else
                {
                    var identity = new ClaimsIdentity(
                        new GenericIdentity(userDTO.Email),
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, userDTO.Email)
                        });

                    var createDate = DateTime.Now;
                    DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    string token = CreateToken(identity, createDate, expirationDate, handler);
                    return SuccessObject(createDate, expirationDate, token, userDTO);
                }
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate,
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, String token, LoginDTO userDTO)
        {
            return new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = token,
                userName = userDTO.Email,
                message = "Usuário logado com sucesso"
            };
        }
    }
}
