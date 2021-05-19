using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Forum.Models
{
    public class ForumAppDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Coment> Coments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public ForumAppDbContext(DbContextOptions<ForumAppDbContext> options)
                    : base(options)
        {
            //Database.EnsureDeleted(); // later switch to Database.Migrate(); - applies migration to db
            //Database.EnsureCreated();
            Database.Migrate();
            //Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coment>().HasOne(p => p.User).WithMany(t => t.Coments).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Coment>().HasOne(p => p.Post).WithMany(t => t.Coments).HasForeignKey(t => t.PostId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Coment>().HasMany(p => p.SubComents).WithOne(t => t.ParentComent).HasForeignKey(t => t.ParentComentId);
            modelBuilder.Entity<Coment>().Property(t => t.Date).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            modelBuilder.Entity<Coment>().Property(t => t.Text).HasMaxLength(300).IsRequired();

            modelBuilder.Entity<Post>().HasOne(p => p.User).WithMany(t => t.Posts).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Post>().HasOne(p => p.Thread).WithMany(t => t.Posts).HasForeignKey(t => t.ThreadId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Post>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.Content).HasMaxLength(100000).IsRequired();
            modelBuilder.Entity<Post>().Property(p => p.Date).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            modelBuilder.Entity<Thread>().Property(p => p.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Thread>().Property(p => p.Description).HasMaxLength(300);

            modelBuilder.Entity<Vote>().HasOne(p => p.Post).WithMany(t => t.Votes).HasForeignKey(t => t.PostId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Vote>().HasOne(p => p.User).WithMany(t => t.Votes).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Chat>().HasOne(m => m.Creator).WithMany(t => t.MyChats).HasForeignKey(m => m.CreatorId).OnDelete(DeleteBehavior.SetNull); 
            modelBuilder.Entity<Chat>().HasOne(m => m.Added).WithMany(t => t.OtherChats).HasForeignKey(m => m.AddedId);
            modelBuilder.Entity<Chat>().Property(p => p.Name).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<Message>().HasOne(p => p.User).WithMany(t => t.Messages).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Message>().HasOne(p => p.Chat).WithMany(t => t.Messages).HasForeignKey(t => t.ChatId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Message>().Property(p => p.Text).HasMaxLength(300).IsRequired();


            modelBuilder.Entity<Subscription>().HasOne(p => p.Thread).WithMany(t => t.Subscriptions).HasForeignKey(t => t.ThreadId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Subscription>().HasOne(p => p.User).WithMany(t => t.Subscriptions).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);

            var threadId1 = Guid.NewGuid().ToString();
            var threadId2 = Guid.NewGuid().ToString();

            var admRolId = Guid.NewGuid().ToString();
            var usrRolId = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole {
                    Id = admRolId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole {
                    Id = usrRolId,
                    Name = "User",
                    NormalizedName = "USER"
                },
            });

            var admUserId = Guid.NewGuid().ToString();
            var UserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = admUserId, // primary key
                    UserName = "admin",
                    Email = "admin@example.com",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "admin")
                },
                new User
                {
                    Id = UserId, // primary key
                    UserName = "new",
                    Email = "new@example.com",
                    NormalizedUserName = "NEW",
                    NormalizedEmail = "NEW@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "123456mM-")
                }
            );

            
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = admRolId, // for admin username
                    UserId = admUserId // for admin role
                },
                 new IdentityUserRole<string>
                 {
                     RoleId = usrRolId, // for admin username
                     UserId = UserId // for admin role
                 }
            );

            modelBuilder.Entity<Thread>().HasData
            (
                new Thread
                {
                    Id = threadId1,
                    Name = "CS",
                    Description = "Thread about computer science"
                },
                new Thread
                {
                    Id = threadId2,
                    Name = "Electrical engeneering",
                    Description = "Thread about electrical engeneering"
                }
            );

            var postId1 = Guid.NewGuid().ToString();
            var postId2 = Guid.NewGuid().ToString();
            modelBuilder.Entity<Place>().HasData
            (
                new Place
                {
                    Id = postId1,
                    UserId = UserId,
                    Name = "Little bit about OS",
                    Content = "Here we are going to talk about OS",
                    ThreadId = threadId1
                },
                new Place
                {
                    Id = postId2,
                    UserId = admUserId,
                    Name = "Little bit about ARM architecture",
                    Content = "ARM is beter then x86",
                    ThreadId = threadId2
                }
            );
            postId1 = Guid.NewGuid().ToString();
            postId2 = Guid.NewGuid().ToString();
            modelBuilder.Entity<Event>().HasData
            (
                new Event
                {
                    Id = postId1,
                    UserId = UserId,
                    Name = "Event 1",
                    Content = "First event",
                    ThreadId = threadId1,
                    DateOfEvent = DateTime.Now.AddDays(1)
                },
                new Event
                {
                    Id = postId2,
                    UserId = admUserId,
                    Name = "Event 2",
                    Content = "Secont event",
                    ThreadId = threadId2,
                    DateOfEvent = DateTime.Now.AddDays(1)
                }
            );

            var comentId1 = Guid.NewGuid().ToString();
            var comentId2 = Guid.NewGuid().ToString();
            modelBuilder.Entity<Coment>().HasData
            (
                new Coment
                {
                    Id = comentId1,
                    PostId = postId1,
                    Text = "Realy cool article"
                },
                new Coment
                {
                    Id = comentId2,
                    PostId = postId2,
                    Text = "ARM the best!!"
                }
            );
        }
    }
}
