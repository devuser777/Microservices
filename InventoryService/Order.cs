using System;

namespace InventoryService
{
    internal class Order
    {
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}