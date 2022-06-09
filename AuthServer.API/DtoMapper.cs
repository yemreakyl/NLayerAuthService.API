using AuthServer.Core.DTOs.GetDtos;
using AuthServer.Core.DTOs.ReturnDtos;
using AuthServer.Core.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto,Product>().ReverseMap();
            CreateMap<UserDto,UserApp>().ReverseMap();
        }
        
    }
}
