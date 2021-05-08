using kedzior.io.ConnectionStringConverter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class MySqlForumAppDbContext : ForumAppDbContext
    {
        public MySqlForumAppDbContext(DbContextOptions<ForumAppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseMySQL(AzureMySQL.ToMySQLStandard(Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb")));
        }
    }
}
