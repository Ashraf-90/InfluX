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
   

    public class MetaPagesProfile : Profile
    {
        public MetaPagesProfile()
        {
            CreateMap<MetaPages, MetaPagesDTo>()
                .ForMember(d => d.Id, op => op.MapFrom(src => src.Id))
                .ForMember(d => d.Page, op => op.MapFrom(src => src.Page))
                .ForMember(d => d.EnPageTitle, op => op.MapFrom(src => src.EnPageTitle))
                .ForMember(d => d.ArPageTitle, op => op.MapFrom(src => src.ArPageTitle))
                .ForMember(d => d.EnMetaTitle, op => op.MapFrom(src => src.EnMetaTitle))
                .ForMember(d => d.ArMetaTitle, op => op.MapFrom(src => src.ArMetaTitle))
                .ForMember(d => d.EnMetaDescription, op => op.MapFrom(src => src.EnMetaDescription))
                .ForMember(d => d.ArMetaDescription, op => op.MapFrom(src => src.ArMetaDescription))
                .ReverseMap();
        }

    }
}
