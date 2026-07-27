using Application.Common.Models;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Application.DTOs.AuthDto;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Services
{
    public class AuthService
    {
        private readonly AppDbContext dbContext;
        private readonly JwtOption jwtOption;

        public AuthService(AppDbContext dbContext, JwtOption jwtOption)
        {
            this.dbContext = dbContext;
            this.jwtOption = jwtOption;
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        private string CreateAuthToken(Guid UserId, string Role, Guid ? StoreId=null)
        {
            Guid storeId = StoreId.HasValue ? StoreId.Value : Guid.Empty ;

            var tokenHandler = new JwtSecurityTokenHandler();

            int LifeTimeMinutes = Role==eUserTypes.Person.ToString()? 40: jwtOption.Lifetime;

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = jwtOption.Issuer,
                Audience = jwtOption.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SigningKey))
                ,SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(LifeTimeMinutes),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier,UserId.ToString()),
                    new (ClaimTypes.Role,Role),
                    new ("store_id",storeId.ToString())
                })
            };

            var SecurtiyToken = tokenHandler.CreateToken(tokenDescriptor);

            var accessToken = tokenHandler.WriteToken(SecurtiyToken);

            return accessToken;
        }

        public async Task<TokenDto> CreateToken(Guid PersonId, Guid UserId, string Role, Guid? StoreId = null)
        {
            string authToken = CreateAuthToken(UserId, Role, StoreId);

            string reffreshToken = GenerateRefreshToken();

            await dbContext.Persons.Where(P => P.Id == PersonId).ExecuteUpdateAsync(S => S.SetProperty(s => s.RefreshToken, reffreshToken)
                                                                .SetProperty(s => s.ExpiredTokenTime, DateTime.UtcNow.AddDays(7)));

            return new TokenDto()
            {
                AuthToken = authToken,
                ReffreshToken = reffreshToken
            };
        }

        public TokenDto CreateAuthTokenOnly(Guid PersonId, Guid UserId, string Role, Guid? StoreId = null)
        {
            string authToken = CreateAuthToken(UserId, Role, StoreId);

            return new TokenDto()
            {
                AuthToken = authToken,
            };
        }



    }
}
