using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options) {}
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<ProductLike> ProductLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

        // one to one
        modelBuilder.Entity<AppUser>()
            .HasOne(a => a.Cart)
            .WithOne(c => c.AppUser)
            .HasForeignKey<Cart>(c => c.AppUserId);

        // many to one
        modelBuilder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Cart)
            .HasForeignKey(i => i.ShoppingCartId);
            

        modelBuilder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedUserId });

        modelBuilder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserLike>()
            .HasOne(s => s.LikedUser)
            .WithMany(l => l.LikedByUsers)
            .HasForeignKey(s => s.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<ProductLike>()
            .HasKey(pl => new { pl.SourceUserId, pl.LikedProductId }); 

        modelBuilder.Entity<ProductLike>()
            .HasOne(pl => pl.SourceUser)
            .WithMany(u => u.LikedProducts)
            .HasForeignKey(pl => pl.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductLike>()
            .HasOne(pl => pl.LikedProduct)
            .WithMany(p => p.ProductLikes) 
            .HasForeignKey(pl => pl.LikedProductId)
            .OnDelete(DeleteBehavior.Cascade);

                        // AppUser to AppRole
            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}