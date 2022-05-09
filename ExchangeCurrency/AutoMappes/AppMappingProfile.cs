using AutoMapper;
using ExchangeCurrency.Data.Models;
using ExchangeCurrency.DtoModels;

namespace ExchangeCurrency.AutoMappes
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Data.Models.Transaction, DtoModels.TransactionDto>();
            CreateMap< DtoModels.TransactionDto, Data.Models.Transaction>();
            CreateMap<Data.Models.CurrencyExchangeRate, DtoModels.CurrencyExchangeRateDto>();
        }
    }
}
