using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SvelteShoppingList.API.Data;
using SvelteShoppingList.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// For Svelte
builder.Services.AddCors(options =>
{
    options.AddPolicy("SvelteApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// So Svelte can use the API
app.UseCors("SvelteApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGroup("/api/shoppinglist").MapShoppingListEndpoints();

app.Run();
