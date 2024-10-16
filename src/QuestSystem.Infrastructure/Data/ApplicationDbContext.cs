using Microsoft.EntityFrameworkCore;
using QuestSystem.Domain.Entities;
using QuestSystem.Domain.ValueObjects;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace QuestSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<PlayerQuest> PlayerQuests { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }
        public DbSet<Item> Items { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Level).IsRequired();
            });

            modelBuilder.Entity<Quest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.MinimumLevel).IsRequired();
                entity.Ignore(e => e.Conditions);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TypeId).IsRequired();
            });

            modelBuilder.Entity<PlayerQuest>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Player)
                      .WithMany(p => p.PlayerQuests)
                      .HasForeignKey(e => e.PlayerId);
                entity.HasOne(e => e.Quest)
                      .WithMany()
                      .HasForeignKey(e => e.QuestId);
                entity.Ignore(e => e.Progress);
            });

            modelBuilder.Entity<PlayerItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Player)
                      .WithMany(p => p.PlayerItems)
                      .HasForeignKey(e => e.PlayerId);
                entity.HasOne(e => e.Item)
                      .WithMany()
                      .HasForeignKey(e => e.ItemId);
            });

            modelBuilder.Entity<QuestCondition>()
                .HasNoKey();

            modelBuilder.Entity<QuestProgress>()
                .HasNoKey();
        }
    }
}
