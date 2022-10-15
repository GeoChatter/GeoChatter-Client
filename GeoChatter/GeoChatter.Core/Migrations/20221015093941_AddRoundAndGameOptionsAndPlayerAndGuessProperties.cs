using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoChatter.Core.Migrations
{
    public partial class AddRoundAndGameOptionsAndPlayerAndGuessProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapRoundSettingsId",
                table: "Round",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupabaseId",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Layer",
                table: "Guess",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Guess",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MapGameSettingsId",
                table: "Game",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapGameSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MapID = table.Column<string>(type: "TEXT", nullable: true),
                    MapName = table.Column<string>(type: "TEXT", nullable: true),
                    IsInfinite = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsStreak = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameType = table.Column<string>(type: "TEXT", nullable: true),
                    GameMode = table.Column<string>(type: "TEXT", nullable: true),
                    GameState = table.Column<string>(type: "TEXT", nullable: true),
                    RoundCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    ForbidMoving = table.Column<bool>(type: "INTEGER", nullable: false),
                    ForbidZooming = table.Column<bool>(type: "INTEGER", nullable: false),
                    ForbidRotating = table.Column<bool>(type: "INTEGER", nullable: false),
                    StreakType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapGameSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapRoundSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoundNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMultiGuess = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Layers = table.Column<string>(type: "TEXT", nullable: true),
                    Is3dEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    BlackAndWhite = table.Column<bool>(type: "INTEGER", nullable: false),
                    Blurry = table.Column<bool>(type: "INTEGER", nullable: false),
                    Mirrored = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpsideDown = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sepia = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxZoomLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRoundSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Round_MapRoundSettingsId",
                table: "Round",
                column: "MapRoundSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_MapGameSettingsId",
                table: "Game",
                column: "MapGameSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_MapGameSettings_MapGameSettingsId",
                table: "Game",
                column: "MapGameSettingsId",
                principalTable: "MapGameSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Round_MapRoundSettings_MapRoundSettingsId",
                table: "Round",
                column: "MapRoundSettingsId",
                principalTable: "MapRoundSettings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_MapGameSettings_MapGameSettingsId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Round_MapRoundSettings_MapRoundSettingsId",
                table: "Round");

            migrationBuilder.DropTable(
                name: "MapGameSettings");

            migrationBuilder.DropTable(
                name: "MapRoundSettings");

            migrationBuilder.DropIndex(
                name: "IX_Round_MapRoundSettingsId",
                table: "Round");

            migrationBuilder.DropIndex(
                name: "IX_Game_MapGameSettingsId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "MapRoundSettingsId",
                table: "Round");

            migrationBuilder.DropColumn(
                name: "SupabaseId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Layer",
                table: "Guess");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Guess");

            migrationBuilder.DropColumn(
                name: "MapGameSettingsId",
                table: "Game");
        }
    }
}
