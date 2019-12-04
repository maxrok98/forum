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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
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

            //modelBuilder.Entity<User>().HasOne(p => p.Image)
            //                           .WithOne(t => t.User)
            //                           .HasForeignKey<UserImage>(t => t.UserId);
            modelBuilder.Entity<UserImage>().Property(i => i.Image).HasMaxLength(10000000);
            modelBuilder.Entity<UserImage>().HasOne(p => p.User).WithOne(t => t.Image).HasForeignKey<User>(t => t.ImageId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PostImage>().Property(i => i.Image).HasMaxLength(10000000);
            modelBuilder.Entity<PostImage>().HasOne(p => p.Post).WithOne(t => t.Image).HasForeignKey<Post>(p => p.ImageId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ThreadImage>().Property(i => i.Image).HasMaxLength(10000000);
            modelBuilder.Entity<ThreadImage>().HasOne(p => p.Thread).WithOne(t => t.Image).HasForeignKey<Thread>(p => p.ImageId).OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<Post>().HasData
            (
                new Post
                {
                    Id = "ertioow",
                    Name = "Little bit about OS",
                    Content = "Here we are going to talk about OS",
                    ThreadId = "sadfasdfa"
                },
                new Post
                {
                    Id = "dfgm,ndsl",
                    Name = "Little bit about ARM architecture",
                    Content = "ARM is beter then x86",
                    ThreadId = "teewrsl"
                }
            );

            modelBuilder.Entity<Coment>().HasData
            (
                new Coment
                {
                    Id = "weorowo",
                    PostId = "ertioow",
                    Text = "Realy cool article"
                },
                new Coment
                {
                    Id = "xcvzxcm,",
                    PostId = "dfgm,ndsl",
                    Text = "ARM the best!!"
                }
            );
        }
    }
}
