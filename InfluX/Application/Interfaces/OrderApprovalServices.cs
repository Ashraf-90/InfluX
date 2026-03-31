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
    public class OrderApprovalServices
        : BaseCrudServices<OrderApproval, OrderApprovalDto, OrderApprovalCreateDto, OrderApprovalUpdateDto>,
          IOrderApprovalServices
    {
        public OrderApprovalServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.OrderApprovals) { }

        protected override Guid GetUpdateId(OrderApprovalUpdateDto dto) => dto.Id;
    }
}
