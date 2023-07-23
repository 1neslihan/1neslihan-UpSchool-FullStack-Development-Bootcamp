using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetByDate
{
    public class OrderGetByDateQuery:IRequest<List<OrderGetByDateDto>>
    {
        public OrderGetByDateQuery(bool? isDeleted /*, string userId, DateTime? startDate, DateTime? endDate*/)
        {
            IsDeleted=isDeleted;
            //UserId=userId;
            //StartDate=startDate;
            //EndDate=endDate;
        }

        public bool? IsDeleted { get; set; }
        //public string  UserId { get; set; }
        //public DateTimeOffset? StartDate { get; set; }
        //public DateTimeOffset? EndDate { get; set; }
    }
}
