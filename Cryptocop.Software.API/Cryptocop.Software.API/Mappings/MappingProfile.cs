using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Entities;

namespace Cryptocop.Software.API.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<AddressInputModel, Address>();
            CreateMap<Address, AddressDto>();
            CreateMap<PaymentCardInputModel, PaymentCard>();
            CreateMap<PaymentCard, PaymentCardDto>();
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
                .ForMember(
                    dest => dest.TotalPrice,
                    opt => opt.MapFrom<TotalPriceResolver>()
                );
        }
    }
}