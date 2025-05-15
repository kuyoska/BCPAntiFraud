# BCP AutiFraud code challenge

This repo contains an simple implementation to solve the BCP Antifraud code challenge.

Stack technology:
* .Net 8
* Docker for containers
* Kafka
* InMemoryDb

## Getting Started

You will need a certain tools in order to locally install the app

### Prerequisites

* Docker for Windows
* Visual Studio 2022

## Solution Description

The Visual Studio solution for this demo contains 6 projects listed here:

 <img src="https://github.com/kuyoska/BCPAntiFraud/blob/master/Images/ProjectSolution.png" alt="solution" >

* BCP.Api.AntiFraud, a web api service that consume kafka messages when a new transaction is created. It will validate according the challenge rules and will return the transaction status(APPROVED, REJECTED)
* BCP.Api.DbService, a web api service that will take care of storing transactions into the In memory DB
* BCP.Api.TransactionService, a web api service that initiates the transaction creation and produce a kafka message to validate the transaction
* BCP.Data, simple db context and entities utilized in the solution
* BCP.Helpers, simple project for helpers like a httpclientwrapper 
* BCP.Services, service to consume the db service web api

I am using a Producer-Consumer strategy with kafka, where the producer(BCP.Api.TransactionService) will create a message with the transaction and a consumer(BCP.Api.AntiFraud) will process the message and produce a new message with the transaction status result , so that another consumer(BCP.Api.TransactionService) reads that message and update the database with the new status.

Comunication with the db is via HTTP using BCP.Services.DbService, so BCP.Api.AntiFraud and BCP.Api.TransactionService comunnicate witht he db using that service.
