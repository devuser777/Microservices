using MassTransit;
using System;

namespace OrderService.Models
{
    public class Order
    {

        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
