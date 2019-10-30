using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Forum.Models
{
    public class AppContext : IdentityDbContext<User>
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Coment> Coments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Thread> Threads { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public AppContext(DbContextOptions<AppContext> options)
                    : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Chat>().HasOne(m => m.Creator)
                                        .WithMany(t => t.MyChats)
                                        .HasForeignKey(m => m.CreatorId);                        
            modelBuilder.Entity<Chat>().HasOne(m => m.Added)
                                        .WithMany(t => t.OtherChats)
                                        .HasForeignKey(m => m.AddedId);
            modelBuilder.Entity<ThreadImage>().Property(i => i.Image)
                                                .HasMaxLength(10000000);
            modelBuilder.Entity<UserImage>().Property(i => i.Image)
                                                .HasMaxLength(10000000);
            modelBuilder.Entity<PostImage>().Property(i => i.Image)
                                                .HasMaxLength(10000000);

            modelBuilder.Entity<Thread>().HasData
            (
                new Thread
                {
                    Id = "sadfasdfa",
                    Name = "CS",
                    Description = "Thread about computer science"
                },
                new Thread
                {
                    Id = "teewrsl",
                    Name = "Electrical engeneering",
                    Description = "Thread about electrical engeneering"
                }
            );
        }
    }
}
