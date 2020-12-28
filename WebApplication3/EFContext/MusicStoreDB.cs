using MusicStoree.Models;
using System.Data.Entity;

namespace MusicStoree.EFContext
{
    public class MusicStoreDB : DbContext
    {
        public MusicStoreDB() : base("DefaultConnection")
        {

        }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}