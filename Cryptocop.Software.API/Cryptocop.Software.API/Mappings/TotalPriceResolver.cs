using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Repositories.Entities;

namespace Cryptocop.Software.API.Mappings
{
    public class TotalPriceResolver : IValueResolver<ShoppingCartItem, ShoppingCartItemDto, double>
    {
        public double Resolve(ShoppingCartItem source, ShoppingCartItemDto destination, double destMember, ResolutionContext context)
        {
            return source.Quantity * source.UnitPrice;
        }
    }
}