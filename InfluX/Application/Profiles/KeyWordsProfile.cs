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
            // Read (DTO)
            CreateMap<KeyWords, KeyWordsDto>().ReverseMap();

            // Create
            CreateMap<KeyWords, KeyWordsCreateDto>().ReverseMap();

            // Update
            CreateMap<KeyWords, KeyWordsUpdateDto>().ReverseMap();
        }
    }
}

