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

        public async Task<Guid?> CreateSellerForFirstTimeAsManager(Guid PersonId)
        {
            Seller seller = new Seller()
            {
                PersonId = PersonId,
                SellerRoleId = (int)eSellerRoles.Manager
            };

            await _appDbContext.AddAsync(seller);

            if (await _appDbContext.SaveChangesAsync() <= 0)
                return null;

            return seller.Id;
        }
        
        //this function is prevent same person to create more than one seller object.
        private async Task<bool> IsSellerHasRelationWithPersonId(Guid PersonId)
        {
            return await _appDbContext.Sellers.Where(S => S.PersonId == PersonId).AnyAsync();
        }

        public async Task<Guid?> HandleCreateSellerForFirstTimeAsManager(Guid PersonId,AddInitialPersonInfoDto addInitialPersonInfoDto)
        {
            if (await IsSellerHasRelationWithPersonId(PersonId))
                return null;

            if (!await _personService.AddInitialPersonInfo(PersonId, addInitialPersonInfoDto))
                return null;

            Guid? Id = await CreateSellerForFirstTimeAsManager(PersonId);

           return Id;
        }

        public async Task<PersonInfoDto?> GetSellerInfo(Guid sellerId)
        {
            Guid personId = await _appDbContext.Sellers.Where(s=>s.Id==sellerId)
                .Select(S => S.PersonId)
               .SingleAsync();

            return await _personService.GetPersonInfo(personId);
        }

        public async Task<bool> HandleUpdateSellerInfo(UpdatePersonDto updatePersonDto,Guid SellerId)
        {
            Guid PersonId = await _appDbContext.Sellers.Where(s=>s.Id==SellerId)
                .Select(s=>s.PersonId)
                .FirstAsync();

            bool isDone = await _personService.UpdatePersonInfo(PersonId,updatePersonDto);

            return isDone;
        }
    }
}
