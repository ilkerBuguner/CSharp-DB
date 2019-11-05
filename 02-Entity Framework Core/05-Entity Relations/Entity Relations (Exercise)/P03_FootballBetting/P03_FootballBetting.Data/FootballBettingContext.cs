using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data
{
     public class FootballBettingContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.DefaultConnection);
            }
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(k => k.TeamId);

                entity.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(p => p.LogoUrl)
                .HasMaxLength(250)
                .IsRequired(false)
                .IsUnicode(false);

                entity.Property(p => p.Initials)
                .HasMaxLength(3)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(p => p.Budget)
                .IsRequired(true);

                entity.HasOne(t => t.PrimaryKitColor)
                .WithMany(k => k.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.SecondaryKitColor)
                .WithMany(k => k.SecondaryKitTeams)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Town)
                .WithMany(to => to.Teams)
                .HasForeignKey(k => k.TownId);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(k => k.ColorId);

                entity.Property(c => c.Name)
                .HasMaxLength(30)
                .IsRequired(true)
                .IsUnicode(true);
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.HasKey(t => t.TownId);

                entity.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity.HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(k => k.CountryId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(k => k.CountryId);

                entity.Property(c => c.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(k => k.PlayerId);

                entity.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(p => p.SquadNumber)
                .IsRequired(true);

                entity.Property(p => p.IsInjured)
                .IsRequired(true);

                entity.HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

                entity.HasOne(p => p.Position)
                .WithMany(po => po.Players)
                .HasForeignKey(p => p.PositionId);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(k => k.PositionId);

                entity.Property(p => p.Name)
                .HasMaxLength(30)
                .IsRequired(true)
                .IsUnicode(false);
            });

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(k => new { k.GameId, k.PlayerId });

                entity.Property(ps => ps.ScoredGoals)
                .IsRequired(true);

                entity.Property(ps => ps.Assists)
                .IsRequired(true);

                entity.Property(ps => ps.MinutesPlayed)
                .IsRequired(true);

                entity.HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);

                entity.HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(k => k.GameId);

                entity.Property(g => g.HomeTeamGoals)
                .IsRequired(true);

                entity.Property(g => g.AwayTeamGoals)
                .IsRequired(true);

                entity.Property(g => g.DateTime)
                .IsRequired(true);

                entity.Property(g => g.HomeTeamBetRate)
                .IsRequired(true);

                entity.Property(g => g.AwayTeamBetRate)
                .IsRequired(true);

                entity.Property(g => g.DrawBetRate)
                .IsRequired(true);

                entity.Property(g => g.Result)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity.HasOne(g => g.HomeTeam)
                .WithMany(ht => ht.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.AwayTeam)
                .WithMany(at => at.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Bet>(entity =>
            {
                entity.HasKey(b => b.BetId);

                entity.Property(b => b.Amount)
                .IsRequired(true);

                entity.Property(b => b.Prediction)
                .HasMaxLength(30)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(b => b.DateTime)
                .IsRequired(true);

                entity.HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);

                entity.HasOne(b => b.Game)
                .WithMany(g => g.Bets)
                .HasForeignKey(b => b.GameId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired(true);

                entity.Property(u => u.Password)
                .HasMaxLength(30)
                .IsRequired(true)
                .IsUnicode(false);

                entity.Property(u => u.Email)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(false);

                entity.Property(u => u.Name)
                .HasMaxLength(30)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(u => u.Name)
                .IsRequired(true);
            });
        }
    }
}
