using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class PixelProfile : Profile
    {
        public PixelProfile()
        {
            CreateMap<Pixels, PixelsDTo>()
                .ForMember(d => d.Id, op => op.MapFrom(src => src.Id))
                .ForMember(d => d.PixelsApp, op => op.MapFrom(src => src.PixelsApp))
                .ForMember(d => d.PixelsCode, op => op.MapFrom(src => src.PixelsCode))
                .ReverseMap();
        }
    }
}
