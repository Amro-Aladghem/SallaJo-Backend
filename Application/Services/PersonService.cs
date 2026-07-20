using Application.DTOs.AuthDto;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonService
    {
        private readonly AppDbContext _appDbContext;
        private readonly PasswordService _passwordService;
        
        public PersonService(AppDbContext appDbContext,PasswordService passwordService)
        {
            _appDbContext = appDbContext;
            _passwordService = passwordService;
        }

        private async Task<Person?> GetAndCheckPerson(PersonAuthDto personAuthDto)
        {
            Person? person  = await _appDbContext.Persons
                .Where(p => p.Phone == personAuthDto.Phone && p.IsActive == true)
                .FirstOrDefaultAsync();

            return person;
        }

        public async Task<PersonAuthResponseDto?> Login(PersonAuthDto personAuthDto)
        {
            Person? person  = await GetAndCheckPerson(personAuthDto);

            if (person == null) return null;

            if (!_passwordService.VerifyEncrypt(person.Password, personAuthDto.Password))
                return null;

            return new PersonAuthResponseDto()
            {

                SysId = person.Id.ToString(),
                ImageUrl = person.ImageUrl,
                FullName = person.FirstName + ' ' + person.LastName,
                IsActive = person.IsActive,
                UserTypeId = person.UserTypeId
            };
        }

        public async Task<PersonAuthResponseDto?> Register(PersonAuthDto personAuthDto)
        {
            string encryptedPassword = _passwordService.Encrypt(personAuthDto.Password);

            Person person = new Person()
            {
                Phone = personAuthDto.Phone,
                Password = encryptedPassword,
                IsActive = false,
            };

            await _appDbContext.Persons.AddAsync(person);

            if (await _appDbContext.SaveChangesAsync() <= 0)
                throw new Exception("Failed to register person");

            return new PersonAuthResponseDto()
            {
                SysId = person.Id.ToString(),
                ImageUrl = null,
                FullName = "",
                IsActive = person.IsActive,
                UserTypeId = person.UserTypeId
            };
        }


        public async Task<PersonAuthResponseDto?> GetPersonInfoWithReffreshToken(string ReffreshToken)
        {
            PersonAuthResponseDto? personAuthResponseDto = await _appDbContext.Persons
                .Where(P => P.RefreshToken == ReffreshToken && P.ExpiredTokenTime > DateTime.UtcNow)
                .Select(person => new PersonAuthResponseDto()
                {
                    SysId = person.Id.ToString(),
                    ImageUrl = null,
                    FullName = "",
                    IsActive = person.IsActive,
                    UserTypeId = person.UserTypeId
                })
                .FirstOrDefaultAsync();

            return personAuthResponseDto;
        }
    }
}
