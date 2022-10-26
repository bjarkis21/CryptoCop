using System;
using System.Text.Json.Serialization;
using Cryptocop.Software.API.Extensions;
using Cryptocop.Software.API.Middlewares;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Implementations;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Implementations;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CryptoCopDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString"),
    b => b.MigrationsAssembly("Cryptocop.Software.API"));
});

builder.Services.AddHttpClient<IExchangeService, ExchangeService>(client => {
    client.BaseAddress = new Uri("https://data.messari.io/api/");
});

builder.Services.AddHttpClient<ICryptoCurrencyService, CryptoCurrencyService>(client => {
    client.BaseAddress = new Uri("https://data.messari.io/api/");
});

builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>(client => {
    client.BaseAddress = new Uri("https://data.messari.io/api/");
});

builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtTokenAuthentication(builder.Configuration);

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtConfig = builder.Configuration.GetSection("JwtConfig");
builder.Services.AddTransient<ITokenService>((c) =>
    new TokenService(
        jwtConfig.GetValue<string>("secret"),
        jwtConfig.GetValue<string>("expirationInMinutes"),
        jwtConfig.GetValue<string>("issuer"),
        jwtConfig.GetValue<string>("audience")));

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();