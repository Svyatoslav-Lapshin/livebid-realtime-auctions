using LiveBid.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using LiveBid.Application.Common.Interfaces;
using LiveBid.Application.Features.Auctions.CreateAuction;
using realtime_auction_platform.EndPoints.Auctions;
using LiveBid.Application.Features.Auctions.GetAuctionById;
using LiveBid.Application.Features.Auctions.GetLiveAuctions;
using LiveBid.Application.Features.Auctions.UpdateAuction;
using LiveBid.Application.Features.Auctions.CancelAuction;
using LiveBid.Application.Features.Auctions.PlaceBid;
using LiveBid.Application.Features.Auctions.StartAuction;
using LiveBid.Application.Features.Auctions.EndAuction;


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
builder.Services.AddScoped<GetLiveAuctionsHandler>();
builder.Services.AddScoped<UpdateAuctionHandler>();
builder.Services.AddScoped<CancelAuctionHandler>();
builder.Services.AddScoped<IValidator<CancelAuctionCommand>, CancelAuctionValidator>();
builder.Services.AddScoped<StartAuctionHandler>();
builder.Services.AddScoped<IValidator<StartAuctionCommand>, StartAuctionValidator>();
builder.Services.AddScoped<EndAuctionHandler>();
builder.Services.AddScoped<IValidator<EndAuctionCommand>, EndAuctionValidator>();
builder.Services.AddScoped<PlaceBidHandler>();
builder.Services.AddScoped<
    IValidator<PlaceBidCommand>,
    PlaceBidValidator>();

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
app.MapGetLiveAuctionsEndpoint();
app.MapUpdateAuctionEndpoint();
app.MapCancelAuctionEndpoint();
app.MapPlaceBidEndpoint();
app.MapStartAuctionEndpoint();
app.MapEndAuctionEndpoint();
app.Run();