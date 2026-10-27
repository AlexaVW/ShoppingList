using Microsoft.EntityFrameworkCore;
using SvelteShoppingList.API.Entities;

namespace SvelteShoppingList.API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
    }
}
