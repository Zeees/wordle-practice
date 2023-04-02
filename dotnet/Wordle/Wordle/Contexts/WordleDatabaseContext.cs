using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Contexts.Configurations;
using Wordle.Models.Database;

namespace Wordle.Contexts
{
    public class WordleDatabaseContext : DbContext
    {
        public DbSet<DbWordleGameInfo> WordleGameInfoEntires { get; set; }

        public WordleDatabaseContext(DbContextOptions<WordleDatabaseContext> options): base(options)
        {
            if(WordleGameInfoEntires== null) {  throw new ArgumentNullException(nameof(WordleGameInfoEntires)); }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new DbWordleGameInfoConfiguration());
        }
    }
}
