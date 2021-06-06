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
        public DbSet<Calendar> Calendar { get; set; }
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

            modelBuilder.Entity<RefreshToken>().HasOne(u => u.User).WithOne(u => u.RefreshToken).OnDelete(DeleteBehavior.Cascade);


            var admRolId = "e8c6906e-c1c0-43fa-aa89-034ec2e6961b";
            var usrRolId = "8642a250-3c71-4e43-9b9d-090f836c6c08";
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

            var admUserId = "5736d00c-ee3f-4ea8-b965-d5a21642d06a";
            var UserId = "dde8b42a-591c-46e1-9de9-49be6442583e";
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

            var threadId1 = "a126c861-36b8-4823-8d4f-65dd12e02b23";
            var threadId2 = "a897c53c-54a2-43c5-a914-326d1ef2d2bc";

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

            var postId1 = "3494f2c5-4966-44c9-bcaa-4360daf44c96";
            var postId2 = "0f8b5f51-856f-467d-ac1d-29647ad68658";
            modelBuilder.Entity<Place>().HasData
            (
                new Place
                {
                    Id = postId1,
                    UserId = UserId,
                    Name = "Little bit about OS",
                    Content = "Here we are going to talk about OS",
                    Latitude = 48.286507f,
                    Longitude = 25.937176f,
                    ThreadId = threadId1
                },
                new Place
                {
                    Id = postId2,
                    UserId = admUserId,
                    Name = "Little bit about ARM architecture",
                    Content = "ARM is beter then x86",
                    Latitude = 48.286506f,
                    Longitude = 25.937170f,
                    ThreadId = threadId2
                }
            );
            var postId3 = "49dd1460-4b04-4249-a32d-282fcf54ff29";
            var postId4 = "8fa4c18f-1c26-4738-879b-31ce028392ed";
            modelBuilder.Entity<Event>().HasData
            (
                new Event
                {
                    Id = postId3,
                    UserId = UserId,
                    Name = "Event 1",
                    Content = "First event",
                    Latitude = 48.286509f,
                    Longitude = 25.937175f,
                    ThreadId = threadId1,
                    DateOfEvent = DateTime.Now.AddDays(1)
                },
                new Event
                {
                    Id = postId4,
                    UserId = admUserId,
                    Name = "Event 2",
                    Content = "Secont event",
                    Latitude = 48.286500f,
                    Longitude = 25.937166f,
                    ThreadId = threadId2,
                    DateOfEvent = DateTime.Now.AddDays(1)
                }
            );

            var comentId1 = "c1c09a1b-9c36-4e6b-a24d-4b7934fab507";
            var comentId2 = "c58de694-692c-4ba3-a746-3114af9f6196";
            modelBuilder.Entity<Coment>().HasData
            (
                new Coment
                {
                    Id = comentId1,
                    PostId = postId1,
                    UserId = UserId,
                    Text = "Realy cool article"
                },
                new Coment
                {
                    Id = comentId2,
                    PostId = postId2,
                    UserId = admUserId,
                    Text = "ARM the best!!"
                }
            );
        }
    }
}
