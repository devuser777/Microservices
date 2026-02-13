# Microservices
.NET core microservices with RabbitMQ

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


<img width="1206" height="1012" alt="image" src="https://github.com/user-attachments/assets/9c9462c3-50a9-4bd9-827d-5955b4bec77d" />

