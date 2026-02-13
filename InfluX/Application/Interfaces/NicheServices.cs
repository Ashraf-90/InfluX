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
    public class NicheServices : BaseCrudServices<Niche, NicheDto, NicheCreateDto, NicheUpdateDto>, INicheServices
    {
        public NicheServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.Niches) { }

        protected override int EFId(Niche entity) => entity.Id;
        protected override int GetUpdateId(NicheUpdateDto dto) => dto.Id;
    }
}

