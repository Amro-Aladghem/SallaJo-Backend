using Application.DTOs.PersonDto;
using Application.DTOs.SellerDto;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SellerService
    {
        private readonly AppDbContext _appDbContext;
        private readonly PersonService _personService;

        public SellerService(AppDbContext appDbContext,PersonService personService)
        {
            _appDbContext = appDbContext;
            _personService = personService;
        }

        public async Task<SellerAuthInfoDto?> GetSellerAuthInfoByPersonId(Guid PersonId)
        {
            SellerAuthInfoDto? sellerAuthInfoDto = await _appDbContext.Sellers
                .Where(S => S.PersonId == PersonId && S.Person.IsActive==true)
                .Select(S=>new SellerAuthInfoDto()
                {
                    Id= S.Id,
                    SellerRoleId=S.SellerRoleId,
                    PersonId= S.PersonId
                })
                .SingleOrDefaultAsync();

            return sellerAuthInfoDto;
        }

        public async Task<bool> CreateSellerForFirstTimeAsManager(Guid PersonId)
        {
            Seller seller = new Seller()
            {
                PersonId = PersonId,
                SellerRoleId = (int)eSellerRoles.Manager
            };

            await _appDbContext.AddAsync(seller);

            return await _appDbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> HandleCreateSellerForFirstTimeAsManager(Guid PersonId,AddInitialPersonInfoDto addInitialPersonInfoDto)
        {
            if (!await _personService.AddInitialPersonInfo(PersonId, addInitialPersonInfoDto))
                return false;

            if (!await CreateSellerForFirstTimeAsManager(PersonId))
                return false;

            return true;
        }
    }
}
