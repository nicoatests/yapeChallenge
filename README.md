# Challenge

## Description

### Anti-fraud

Every time a financial transaction is created, it must be validated by our **Anti-Fraud Microservice**.  
Once validated, the same service sends a message back to update the transaction status.

Currently, we have **three transaction statuses**:

1. `pending`  
2. `approved`  
3. `rejected`

Rejection Criteria:
- Every transaction with a **value greater than 2000**.
- If the **daily accumulated amount** exceeds **20000**.

<img width="253" height="130" alt="image" src="https://github.com/user-attachments/assets/b34d6d30-79f9-469d-a7d6-40f7bcd6116b" />

---

## Tech Stack

- [.NET 8](https://dotnet.microsoft.com/)
- Any SQL database
- Kafka (or RabbitMQ via MassTransit)

> A `Dockerfile` is provided to help you get started with a development environment.

---

## Requirements

You must expose an endpoint to **create a transaction**, containing the following fields:

<img width="333" height="101" alt="image" src="https://github.com/user-attachments/assets/dee0bbba-4cfd-46f4-955e-55226a7d89bd" />

---

## Solution

###Architecture Diagram

<img width="831" height="749" alt="yapediagram drawio" src="https://github.com/user-attachments/assets/813b7948-8583-43bb-8992-68e8e12f46bf" />

### Projects

This repository contains **two microservices**:

| Microservice        | Description                     |
|---------------------|---------------------------------|
| `TransferYape`      | Handles transaction creation and status update. |
| `AntifraudYape`     | Validates the transactions against rejection rules. |

---

### Functionalities

#### 🔹 `TransferYape` (Transaction Service)

1. **POST /api/Transactions**  
   Creates a new transaction with status `"Pending"`.

   **Example:**
   ```http
   POST /api/Transactions
   Content-Type: application/json

   {
       "sourceAccountId": "b34fcb26-3915-4f06-b50c-a8e1e0a74a9b",
       "targetAccountId": "863005d1-50a2-4574-a210-60223024149d",
       "value": 100
   }
   ```

2. **Consumer**:  
   Waits for validation response from the antifraud microservice and updates transaction status.

   - Topic: `transaction-validated`
   - Consumer:  
     `TransferYape.Worker.Transactions.Consumers.TransactionValidatedThenUpdateStatus`

---

#### 🔹 `AntifraudYape` (Anti-Fraud Service)

- **Consumer**:  
  Listens for transaction creation events and applies the rejection criteria.

  - Topic: `transaction-created`
  - Consumer:  
    `AntifraudYape.Worker.Transactions.Consumers.TransactionCreatedThenValidateValue`

---

## Getting Started

Run the following to start the environment using Docker:

```bash
cd path/to/docker-compose.yml
docker compose --env-file .env.local up -d --build
```

---

## Messaging Configuration

The system uses **RabbitMQ**, but it's prepared to work with **Kafka**.

To switch to Kafka:

1. Open `.env.local`
2. Set:
   ```env
   RABBITMQENABLED=false
   KAFKACONNECTIONSTRING=your_kafka_connection_string
   ```

This flexibility is powered by **MassTransit**, which abstracts the messaging system and supports Kafka, RabbitMQ, Azure Service Bus, and more.
