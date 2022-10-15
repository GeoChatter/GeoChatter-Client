using Antlr4.StringTemplate.Compiler;
using CefSharp.DevTools.Browser;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Model.Map;
using GeoChatter.Helpers;
using GeoChatter.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace GeoChatter.Core.Storage
{
    /// <summary>
    /// Client DB sources
    /// </summary>
    public class ClientDbContext : DbContext
    {
        /// <summary>
        /// All players in DB
        /// </summary>
        public DbSet<Player> Players { get; set; }
        /// <summary>
        /// Games
        /// </summary>
        public DbSet<Game> Game { get; set; }
        /// <summary>
        /// GeoGuessr Game results
        /// </summary>
        public DbSet<GeoGuessrGame> GeoGuessrGame { get; set; }
        /// <summary>
        /// Game results
        /// </summary>
        public DbSet<GameResult> GameResults { get; set; }
        /// <summary>
        /// Chat message strings
        /// </summary>
        public DbSet<ChatMessage> ChatMessages { get; set; }
        //public DbSet<GameRound> Rounds { get; set; }
        //public DbSet<Game> Games { get; set; }
        //public DbSet<Guess> Guesses { get; set; }

        /// <summary>
        /// DB file path
        /// </summary>
        public string DbPath { get; }

        /// <summary>
        /// 
        /// </summary>
        public ClientDbContext()
        {
            DbPath = "GeoChatter.db";
        }

        /// <summary>
        /// Migrate with <paramref name="appSettings"/>
        /// </summary>
        /// <param name="appSettings"></param>
        public void Migrate(ApplicationSettingsBase appSettings)
        {
            try
            {
                //   "20220422074950_ChatMessages"
                GCUtils.ThrowIfNull(appSettings, nameof(appSettings));
                List<string> migrations = Database.GetPendingMigrations().ToList();
                Database.Migrate();
                if (migrations != null)
                {
                    SeedData(appSettings, migrations);
                }
            }catch(Exception e)
            {
                string t = e.Message;
            }
        }

        /// <summary>
        /// Seed data
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="migrations"></param>
        public void SeedData(ApplicationSettingsBase appSettings, List<string> migrations)
        {
            GCUtils.ThrowIfNull(appSettings, nameof(appSettings));
            GCUtils.ThrowIfNull(migrations, nameof(migrations));
            if (migrations.FirstOrDefault(s => s.ContainsDefault("initial")) != null)
            {
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_roundStart", Message = "Round <roundNumber> has started! Guesses are open!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_roundEnd", Message = "The round has finished! Congrats to @<winner>!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_guessReveived", Message = "@<playerName> has guessed!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_doubleGuessReveived", Message = "@<playerName>: You have already guessed!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_sameGuessReveived", Message = "@<playerName>: You have pasted your previous guess!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_gameEnd", Message = "The game has finished! Congrats to @<winner>! 🏆 See the result @ https://geochatter.tv/results/?id=<gameId>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_linkMessage", Message = "To play along, go to this link, log-in with Twitch, click on a location and then click the \"Guess\" button: https://geochatter.tv/map/?<botName>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_ExitWithRunningGame", Message = "You have a game running! Are you sure you want to exit?" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_FlagAssignedMessage", Message = "<playerName> got the flag of <name> (<flag>)" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_GuessesOpenedMessage", Message = "Guesses are open" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_GuessesClosedMessage", Message = "Guesses are closed" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_MapExplanationMessage", Message = "@<playerName> You can tilt the map by clicking and dragging with the right mouse button. To change the map layer click the layer icon on the right side. To reset to map, click the compass in the upper right corner!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_GcExplanationMessage", Message = "Thank you for asking, @<playerName>! GeoChatter is an advanced version of chatguessr giving the streamer and you, the viewer, more options. On streamer's side, there are a lot of customizability options; On viewer's side, there is a better map available for guessing and access to more stats!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_FlagRemovedMessage", Message = "<playerName> has requested to have their flag removed!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_BannedStatsRequest", Message = "Sorry, no stats for you. You're banned, <playerName>." });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_ColorAssignedMessage", Message = "<playerName> set their color to <color>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_ColorRemovedMessage", Message = "<playerName> has requested to have their color removed!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_ColorNotFoundMessage", Message = "Sorry @<playerName>, could not find color <color>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_ColorRandomMessage", Message = "<playerName> set their color to random" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_doubleGuessTooFast", Message = "@<playerName>: You are guessing too fast! Please wait at least 10 seconds between guesses!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_joinMessage", Message = "GeoChatter is now connected (<currentTime>)" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_gameStart", Message = "A new game has started! Good luck!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_gameEndNoSummary", Message = "The game has finished! Congrats to @<winner>! 🏆" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_NoRecordsFound", Message = "Sorry <playerName> , no records found for user '<targetName>'" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_NoColor", Message = "Sorry @<playerName> , you don't have a color set. Use \"!c COLOR_NAME\" or \"!c #COLOR_HEX\" to set your color." });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_NoTargetColor", Message = $"Sorry @<playerName> , @<targetName> doesn't have a color set!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_NoTargetFlag", Message = $"Sorry @<playerName> , @<targetName> doesn't have a flag set!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_CurrentColor", Message = "@<playerName> , your current user name color is <color>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_CurrentTargetColor", Message = "@<playerName> , user name color of @<targetName> is <color>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_NoFlag", Message = "<Sorry @playerName>, you don't have a flag set. Use \"!f FLAG_CODE\" or \"!f FLAG_NAME\" to set your flag." });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_CurrentFlagTarget", Message = "@<playerName>, @<targetName>'s flag is <flagName> (<flagCode>)" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_CurrentFlag", Message = "@<playerName>, your current flag is <flagName> (<flagCode>)" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_StatsReset", Message = "@<playerName>: Your stats have been reset." });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_Stats", Message = "@<targetName>'s stats: <msg>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_AvailableFlags", Message = "Available flags: <flagUrl>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_FlagPacks", Message = "Installed flag packs: <packs>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_AvailableFlagPacks", Message = "Available flags in flag packs: <flagUrl>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_Commands", Message = "Available commands: <commandUrl>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_CurrentVersion", Message = "Current version: <version>" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_Colors", Message = "Available username color names: https://docs.microsoft.com/en-us/dotnet/media/art-color-table.png" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_EndStreakRound", Message = "Streaks round <roundNumber> ended. <winnerName> was closest!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_EndStreak", Message = "Streaks game ended at a streak of <endRoundNumber>!" });
                ChatMessages.Add(new ChatMessage { Name = "Chat_Msg_EndStreakSummary", Message = "Streaks game ended at a streak of <endRoundNumber>! Congrats to <winner>! 🏆 See the result @ https://geochatter.tv/results/?id=<gameId>" });
                
                SaveChanges();
            }
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            GCUtils.ThrowIfNull(optionsBuilder, nameof(optionsBuilder));
             optionsBuilder.UseSqlite($"Data Source={DbPath}");
            //optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Initial Catalog=GCDatabase;Integrated Security=true;");
            //optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Integrated Security=true");// ;AttachDbFileName=" + Application.StartupPath + "GCDatabase.mdf");
            //optionsBuilder.EnableSensitiveDataLogging(true).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            //  options.LogTo(message => Debug.WriteLine(message));
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GCUtils.ThrowIfNull(modelBuilder, nameof(modelBuilder));
            #region ChatMessage
         //   modelBuilder.Entity<ChatMessage>();//.HasAlternateKey(e => e.Name);

            #endregion
            #region PLAYER
            modelBuilder.Entity<Player>()
                .Property(b => b.CountryStreak)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.BestStreak)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.CorrectCountries)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.NumberOfCountries)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.Wins)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.Perfects)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.BestGame)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.BestRound)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.NoOfGuesses)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.NoOf5kGuesses)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.RoundNumberOfLastGuess)
                .HasDefaultValue(0);
            modelBuilder.Entity<Player>()
                .Property(b => b.IsBanned)
                .HasDefaultValue(false);
            modelBuilder.Entity<Player>()
                .HasAlternateKey(c => new { c.Id, c.Channel });
            #endregion

            #region GAME
            modelBuilder.Entity<Game>()
                .HasAlternateKey(c => new { c.Id, c.Channel });

            modelBuilder.Entity<Game>()
                .Property(g => g.Flags)
                .HasDefaultValue(GameOption.DEFAULT);

            modelBuilder.Entity<Game>()
                .HasOne(f => f.Next)
                .WithOne(f => f.Previous)
                .HasForeignKey("GeoChatter.Model.Game")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(f => f.Source);
            #endregion

            modelBuilder.Entity<MapRoundSettings>()
            .Property(e => e.Layers)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }


    }
}