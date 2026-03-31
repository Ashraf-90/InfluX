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
    public class OrderDeliverableServices
        : BaseCrudServices<OrderDeliverable, OrderDeliverableDto, OrderDeliverableCreateDto, OrderDeliverableUpdateDto>,
          IOrderDeliverableServices
    {
        public OrderDeliverableServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.OrderDeliverables) { }

        protected override Guid GetUpdateId(OrderDeliverableUpdateDto dto) => dto.Id;
    }
}
