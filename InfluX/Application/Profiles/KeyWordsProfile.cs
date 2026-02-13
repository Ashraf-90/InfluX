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
    public class KeyWordsProfile : Profile
    {
        public KeyWordsProfile()
        {
            CreateMap<KeyWords, KeyWordsDTo>()
                .ForMember(d => d.Id, op => op.MapFrom(src => src.Id))
                .ForMember(d => d.EnKeyword, op => op.MapFrom(src => src.EnKeyword))
                .ForMember(d => d.ArKeyword, op => op.MapFrom(src => src.ArKeyword))
                .ReverseMap();
        }
    }
}
