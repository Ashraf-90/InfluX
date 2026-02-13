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
    public class InfluencerAssetServices : BaseCrudServices<InfluencerAsset, InfluencerAssetDto, InfluencerAssetCreateDto, InfluencerAssetUpdateDto>, IInfluencerAssetServices
    {
        public InfluencerAssetServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.InfluencerAssets) { }

        protected override int EFId(InfluencerAsset entity) => entity.Id;
        protected override int GetUpdateId(InfluencerAssetUpdateDto dto) => dto.Id;
    }
}

