using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Interfaces
{
    public class InfluencerMediaServices : BaseCrudServices<InfluencerMedia, InfluencerMediaDto, InfluencerMediaCreateDto, InfluencerMediaUpdateDto>, IInfluencerMediaServices
    {
        public InfluencerMediaServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.InfluencerMedia) { }

        protected override int EFId(InfluencerMedia entity) => entity.Id;
        protected override int GetUpdateId(InfluencerMediaUpdateDto dto) => dto.Id;
    }
}

