using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Models.Database;

namespace Wordle.Contexts.Configurations
{
    internal class DbWordleGameInfoConfiguration : IEntityTypeConfiguration<DbWordleGameInfo>
    {
        public void Configure(EntityTypeBuilder<DbWordleGameInfo> builder)
        {
            builder.ToTable("wordle_game_info");

            builder.HasKey(x => x.GameId);

            builder.Property(x => x.GameId)
                .IsRequired()
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Word)
                .IsRequired();
            builder.Property(x => x.MaxAttempts)
                .IsRequired();
            builder.Property(x => x.Attempts)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(x => x.IsDone)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(x => x.GuessesJson);
            builder.Property(x => x.CreatedAt)
                .IsRequired();
            builder.Property(x => x.LastUpdatedAt)
                .IsRequired();
        }
    }
}
