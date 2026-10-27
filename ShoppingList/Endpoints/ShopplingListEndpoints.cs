using SvelteShoppingList.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using SvelteShoppingList.API.Entities;

namespace SvelteShoppingList.API.Endpoints
{
    public static class ShopplingListEndpoints
    {
        // WebApplication Extention

        //public static WebApplication MapShoppingListEndpoints(this WebApplication app)
        //{
        //    app.MapGet("/api/shoppinglist", async (ApplicationDbContext context) =>
        //    await context.ShoppingItems.ToListAsync());

        //    app.MapGet("/api/shoppinglist/{id:int}", async (ApplicationDbContext context, int id) =>
        //    {
        //        var item = await context.ShoppingItems.FindAsync(id);
        //        return item is not null ? Results.Ok(item) : Results.NotFound();

        //    });

        //    return app;
        //}

        public static RouteGroupBuilder MapShoppingListEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (ApplicationDbContext context) =>
            await context.ShoppingItems.ToListAsync());

            group.MapGet("/{id}", async (ApplicationDbContext context, int id) =>
            {
                var item = await context.ShoppingItems.FindAsync(id);
                return item is not null ? Results.Ok(item) : Results.NotFound();
            });

            group.MapPost("/", async (ApplicationDbContext context, ShoppingItem newItem) =>
            {
                if (newItem is null)
                    return Results.BadRequest();

                context.ShoppingItems.Add(newItem);
                await context.SaveChangesAsync();

                return Results.Created($"/api/shoppinglist/{newItem.Id}", newItem);
            });

            group.MapPut("/{id}", async (ApplicationDbContext context, int id, ShoppingItem updatedItem) =>
            {
                var item = await context.ShoppingItems.FindAsync(id);
                if (item is null)
                    return Results.NotFound();

                item.Item = updatedItem.Item;
                item.Amount = updatedItem.Amount;

                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            group.MapDelete("/{id}", async (ApplicationDbContext context, int id) =>
            {
                var item = await context.ShoppingItems.FindAsync(id);
                if (item is null)
                    return Results.NotFound();

                context.ShoppingItems.Remove(item);
                await context.SaveChangesAsync();

                return Results.NoContent();
            });

            return group;
        }
    }
}
