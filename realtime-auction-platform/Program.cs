using LiveBid.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Auctions.CreateAuction;
using realtime_auction_platform.EndPoints.Auctions;
using LiveBid.Application.Features.Auctions.GetAuctionById;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<LiveBidDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ILiveBidDbContext>(provider => provider.GetRequiredService<LiveBidDbContext>());
builder.Services.AddScoped<IValidator<CreateAuctionCommand>, CreateAuctionValidator>();
builder.Services.AddScoped<CreateAuctionHandler>();
builder.Services.AddScoped<GetAuctionByIdHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.MapGet("/", () => "LiveBid API is running");

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        Status = "Healthy",
        Service = "LiveBid.Api"
    });
});

app.MapGet("/health/db", async (LiveBidDbContext dbContext) =>
{
    var canConnect = await dbContext.Database.CanConnectAsync();

    return canConnect
        ? Results.Ok(new
        {
            Status = "Healthy",
            Database = "PostgreSQL"
        })
        : Results.Problem("Database connection failed");
});


app.MapCreateAuctionEndpoint();
app.MapGetAuctionByIdEndpoint();

app.Run();