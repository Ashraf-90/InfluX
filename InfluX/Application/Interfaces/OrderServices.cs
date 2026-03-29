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
    public class OrderServices
        : BaseCrudServices<Order, OrderDto, OrderCreateDto, OrderUpdateDto>,
          IOrderServices
    {
        public OrderServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.Orders) { }

        protected override Guid GetUpdateId(OrderUpdateDto dto) => dto.Id;
    }
}
