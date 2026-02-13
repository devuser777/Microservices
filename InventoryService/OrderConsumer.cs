using MassTransit;
using System;
using System.Threading.Tasks;

namespace InventoryService
{
    internal class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            var msg = context.Message;

            await Console.Out.WriteAsync(msg.ToString());
        }
    }
}