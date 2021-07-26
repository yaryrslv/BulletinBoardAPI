using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.EF;
using Data.Models.Realizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.DTO.Ad;
using Web.Services.Abstractions;

namespace Web.Services.Realization
{
    public class AdService : IAdService
    {
        private BulletinBoardContext _context;
        private readonly IMapper _mapper;
        public IConfiguration Configuration { get; }
        public AdService(BulletinBoardContext context, IMapper mapper,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            Configuration = configuration;
        }
        public async Task<IEnumerable<AdFullDto>> GetAllAsync()
        {
            var ads = await _context.Ads.ToListAsync();
            IEnumerable<AdFullDto> adFullDtos; 
            adFullDtos = _mapper.Map(ads, (IEnumerable<AdFullDto>) null);
            return adFullDtos;
        }
        public async Task<IEnumerable<AdFullDto>> GetAllActualAsync()
        {
            var ads = await _context.Ads.Where(i => i.ExpirationDite > DateTime.Now).ToListAsync();
            IEnumerable<AdFullDto> adFullDtos;
            adFullDtos = _mapper.Map(ads, (IEnumerable<AdFullDto>) null);
            return adFullDtos;
        }
        public async Task<AdFullDto> GetByIdAsync(Guid id)
        {
            var ad = await _context.Ads.FindAsync(id);
            AdFullDto adFullDto = _mapper.Map<AdFullDto>(ad);
            return adFullDto; 
        }
        public async Task<AdFullDto> GetByNumberAsync(int number)
        {
            var ad = await _context.Ads.FirstOrDefaultAsync(i => i.Number == number);
            AdFullDto adFullDto;
            adFullDto = _mapper.Map(ad, (AdFullDto) null);
            return adFullDto;
        }
        public async Task<IEnumerable<AdFullDto>> GetByNameAsync(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.UserName == name);
            var ads = await _context.Ads.Where(i => i.UserId == user.Id).ToListAsync();
            IEnumerable<AdFullDto> adFullDtos;
            adFullDtos = _mapper.Map(ads, (IEnumerable<AdFullDto>)null);
            return adFullDtos;
        }
        public async Task<IEnumerable<AdFullDto>> GetByCityAsync(string city)
        {
            var ads = await _context.Ads.Where(i => i.City == city).ToListAsync();
            IEnumerable<AdFullDto> adFullDtos;
            adFullDtos = _mapper.Map(ads, (IEnumerable<AdFullDto>)null);
            return adFullDtos;
        }
        public async Task CreateAsync(AdFullDto adFullDto)
        {
            var user = _context.Users.FirstOrDefault(i => i.Id == adFullDto.UserId);
            adFullDto.CreateDate = DateTime.Now;
            adFullDto.ExpirationDite = adFullDto.CreateDate.AddMonths(1);
            adFullDto.Rating = 0;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var ad = _mapper.Map<Ad>(adFullDto); 
                await _context.Ads.AddAsync(ad); 
                await _context.SaveChangesAsync();

                var adsCount = _context.Ads.Where(i => i.User.Id == user.Id).Count();
                if (adsCount <= int.Parse(Configuration["MaxUserAds"]))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }

            }
        }
        public async Task UpdateAsync(AdFullDto adFullDto)
        {
            var ad = _mapper.Map<Ad>(adFullDto);
            _context.Ads.Update(ad);
            await _context.SaveChangesAsync();
        }
        public async Task<AdFullDto> DeleteAsync(AdFullDto adFullDto)
        {
            var ad = _mapper.Map<Ad>(adFullDto);
            if (ad != null)
            {
                _context.Ads.Remove(ad);
                await _context.SaveChangesAsync();
            }
            return adFullDto;
        }
    }
}
