using Microsoft.EntityFrameworkCore;
using MiniPay.Application.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "MiniPay API",
		Version = "v1",
		Description = "API for MiniPay Platform"
	});
});

// Add CORS
// TODO: Change to proper CORS Settings
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	// Setup SQL Server with connection string from appsettings.json
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register own services using Depndency Injection
builder.Services.AddScoped<MiniPay.Application.Services.IPaymentProviderService, MiniPay.Application.Services.PaymentProviderService>();
builder.Services.AddScoped<MiniPay.Application.Repositories.IPaymentProviderRepository, MiniPay.Application.Repositories.PaymentProviderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
		c.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
	});
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRouting();

app.MapControllers();

app.Run();
