## About

**LiveBid** is an auction platform built with **ASP.NET Core, Entity Framework Core, and PostgreSQL**.

The project currently focuses on the backend and follows a layered architecture with separate **Domain, Application, Infrastructure, API, and Tests** projects. It covers auction lifecycle management, bidding rules, validation, persistence, and integration testing against a real PostgreSQL database.

The backend is being developed incrementally, with business rules kept inside the domain model and application handlers responsible for orchestrating use cases.

A **frontend client is planned as a future stage of the project**. It will consume the existing API and gradually add the user-facing auction experience, including auction browsing, bidding, account-related flows, and real-time functionality.

## Current Status

The core auction and bidding functionality is implemented and covered by integration tests.

### Completed so far

* .NET solution structure with Domain, Application, Infrastructure, API, and Tests projects
* PostgreSQL database integration with EF Core and Npgsql
* Docker Compose setup for PostgreSQL and pgAdmin
* Entity configurations for User, Auction, and Bid
* PostgreSQL snake_case table and column mappings
* Result and Error pattern
* FluentValidation setup
* Auction lifecycle:

  * Draft
  * Scheduled
  * Live
  * Ended
  * Canceled
* Create auction flow
* Get auction by ID
* Start auction
* End auction with winning bid resolution
* Place bid flow with business rules:

  * Auction must be live and active
  * Seller cannot bid on their own auction
  * Bidder must exist
  * Bid amount must be greater than the current price
  * Successful bids update the current auction price
* Paginated auction bid history with sorting
* API and database health checks
* Manual endpoint testing through Postman
* Separate PostgreSQL integration test database
* Integration test infrastructure with automatic database cleanup
* PlaceBid handler integration test coverage for successful and failure scenarios

### Testing

The `PlaceBidHandler` integration test suite currently verifies:

* Successful bid placement
* Bid persistence in PostgreSQL
* Auction current price updates
* Rejection of bids below the current price
* No bid creation after a rejected bid
* No price update after a rejected bid
* Seller cannot bid on their own auction
* Non-existent bidder handling
* Non-existent auction handling
* Scheduled auction rejection
* Ended auction rejection

All current integration tests are passing.

### Planned Development

Future development will focus on:

* Frontend application consuming the LiveBid API
* User-facing auction browsing and bidding flows
* Authentication and authorization
* Real-time auction updates and bidding
* Improved automated test coverage
* Deployment and production configuration
