using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IPublishEndpoint _publishEndpoint { get; set; }

        public OrderController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            // Publish the order created event to the message broker
            await _publishEndpoint.Publish(order);

            return Ok("Order created and event published.");
        }

    }
}

/*
 

client -> makes a call to /order which is routed by API gateway to --> POST /api/order

client -> make a call to /inventory which is routed by API gateway to --> GET /api/inventory

This configuration is done inside ocelot.json file in APIGateway project

OrderService publish an event to RabbitMQ when an order is created. InventoryService consumes that event and reserves the stock.
start docker desktop and start conatiner name- rabbitmq

                 ┌──────────────────────┐
                 │   Frontend / Client   │
                 │ (Web / Mobile / SPA)  │
                 └──────────┬───────────┘
                            │  HTTP
                            ▼
                 ┌──────────────────────┐
                 │     API Gateway       │
                 │   (Ocelot / YARP)     │
                 └───────┬───────┬──────┘
                         │       │
               HTTP      │       │      HTTP
                         │       │
                         ▼       ▼
          ┌──────────────────┐   ┌──────────────────┐
          │   OrderService    │   │   UsersService    │
          │  (Commands/Write) │   │ (Queries/Read etc)│
          └────────┬─────────┘   └──────────────────┘
                   │
                   │  Publish Event (RabbitMQ)
                   ▼
            ┌───────────────────┐
            │     RabbitMQ       │
            │ (Message Broker)   │
            └────────┬──────────┘
                     │ Consume Event
                     ▼
          ┌──────────────────┐
          │ InventoryService  │
          │ (Reserve Stock)   │
          └──────────────────┘

 */
