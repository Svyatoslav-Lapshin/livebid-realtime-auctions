## Current Status

The backend foundation and the first auction features are completed.

Completed so far:

- .NET solution structure with Domain, Application, Infrastructure, API, and Tests projects
- Docker Compose setup for PostgreSQL and pgAdmin
- Initial PostgreSQL schema
- EF Core and Npgsql database connection
- Entity configurations for User, Auction, and Bid
- PostgreSQL snake_case and decimal mappings
- API and database health checks
- Result and Error pattern
- FluentValidation setup
- CreateAuction command, validator, handler, and POST endpoint
- GetAuctionById query, handler, response, and GET endpoint
- Manual endpoint testing through Postman
