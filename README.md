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


<img width="917" height="423" alt="image" src="https://github.com/user-attachments/assets/6dba9333-630b-4ba3-bc69-b438600ddd89" />

<img width="1092" height="432" alt="image" src="https://github.com/user-attachments/assets/3cd2f9a3-c072-4936-9b2d-87bb23a1f788" />

Redis cache implementation:
<img width="1655" height="922" alt="image" src="https://github.com/user-attachments/assets/527ddb5f-977c-44ab-8587-7337fc1d4040" />



