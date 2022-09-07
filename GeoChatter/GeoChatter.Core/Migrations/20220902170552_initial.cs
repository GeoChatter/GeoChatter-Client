using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoChatter.Core.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGMax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGMax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGMeters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    amount = table.Column<string>(type: "TEXT", nullable: true),
                    unit = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGMeters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGMiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    amount = table.Column<string>(type: "TEXT", nullable: true),
                    unit = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGMiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGMin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGMin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGPin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    url = table.Column<string>(type: "TEXT", nullable: true),
                    anchor = table.Column<string>(type: "TEXT", nullable: true),
                    isDefault = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGPin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGRoundScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    amount = table.Column<string>(type: "TEXT", nullable: true),
                    unit = table.Column<string>(type: "TEXT", nullable: true),
                    percentage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGRoundScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GGTotalScore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    amount = table.Column<string>(type: "TEXT", nullable: true),
                    unit = table.Column<string>(type: "TEXT", nullable: true),
                    percentage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGTotalScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MinId = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bounds_Coordinates_MaxId",
                        column: x => x.MaxId,
                        principalTable: "Coordinates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bounds_Coordinates_MinId",
                        column: x => x.MinId,
                        principalTable: "Coordinates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GGTotalDistance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    metersId = table.Column<int>(type: "INTEGER", nullable: true),
                    milesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGTotalDistance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GGTotalDistance_GGMeters_metersId",
                        column: x => x.metersId,
                        principalTable: "GGMeters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GGTotalDistance_GGMiles_milesId",
                        column: x => x.milesId,
                        principalTable: "GGMiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GGBounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    minId = table.Column<int>(type: "INTEGER", nullable: true),
                    maxId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGBounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GGBounds_GGMax_maxId",
                        column: x => x.maxId,
                        principalTable: "GGMax",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GGBounds_GGMin_minId",
                        column: x => x.minId,
                        principalTable: "GGMin",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GGPlayer",
                columns: table => new
                {
                    GcId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    totalScoreId = table.Column<int>(type: "INTEGER", nullable: true),
                    totalDistanceId = table.Column<int>(type: "INTEGER", nullable: true),
                    totalDistanceInMeters = table.Column<double>(type: "REAL", nullable: false),
                    totalTime = table.Column<int>(type: "INTEGER", nullable: false),
                    totalStreak = table.Column<int>(type: "INTEGER", nullable: false),
                    isLeader = table.Column<bool>(type: "INTEGER", nullable: false),
                    currentPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    pinId = table.Column<int>(type: "INTEGER", nullable: true),
                    id = table.Column<string>(type: "TEXT", nullable: true),
                    nick = table.Column<string>(type: "TEXT", nullable: true),
                    isVerified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGPlayer", x => x.GcId);
                    table.ForeignKey(
                        name: "FK_GGPlayer_GGPin_pinId",
                        column: x => x.pinId,
                        principalTable: "GGPin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GGPlayer_GGTotalDistance_totalDistanceId",
                        column: x => x.totalDistanceId,
                        principalTable: "GGTotalDistance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GGPlayer_GGTotalScore_totalScoreId",
                        column: x => x.totalScoreId,
                        principalTable: "GGTotalScore",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeoGuessrGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    token = table.Column<string>(type: "TEXT", nullable: true),
                    type = table.Column<string>(type: "TEXT", nullable: true),
                    mode = table.Column<string>(type: "TEXT", nullable: true),
                    state = table.Column<string>(type: "TEXT", nullable: true),
                    roundCount = table.Column<int>(type: "INTEGER", nullable: false),
                    timeLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    forbidMoving = table.Column<bool>(type: "INTEGER", nullable: false),
                    forbidZooming = table.Column<bool>(type: "INTEGER", nullable: false),
                    forbidRotating = table.Column<bool>(type: "INTEGER", nullable: false),
                    streakType = table.Column<string>(type: "TEXT", nullable: true),
                    map = table.Column<string>(type: "TEXT", nullable: true),
                    mapName = table.Column<string>(type: "TEXT", nullable: true),
                    panoramaProvider = table.Column<int>(type: "INTEGER", nullable: false),
                    boundsId = table.Column<int>(type: "INTEGER", nullable: true),
                    round = table.Column<int>(type: "INTEGER", nullable: false),
                    playerGcId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoGuessrGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoGuessrGame_GGBounds_boundsId",
                        column: x => x.boundsId,
                        principalTable: "GGBounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GeoGuessrGame_GGPlayer_playerGcId",
                        column: x => x.playerGcId,
                        principalTable: "GGPlayer",
                        principalColumn: "GcId");
                });

            migrationBuilder.CreateTable(
                name: "GGGuess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false),
                    timedOut = table.Column<bool>(type: "INTEGER", nullable: false),
                    timedOutWithGuess = table.Column<bool>(type: "INTEGER", nullable: false),
                    roundScoreId = table.Column<int>(type: "INTEGER", nullable: true),
                    roundScoreInPercentage = table.Column<double>(type: "REAL", nullable: false),
                    roundScoreInPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    distanceId = table.Column<int>(type: "INTEGER", nullable: true),
                    distanceInMeters = table.Column<double>(type: "REAL", nullable: false),
                    streakLocationCode = table.Column<string>(type: "TEXT", nullable: true),
                    time = table.Column<int>(type: "INTEGER", nullable: false),
                    GGPlayerGcId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGGuess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GGGuess_GGPlayer_GGPlayerGcId",
                        column: x => x.GGPlayerGcId,
                        principalTable: "GGPlayer",
                        principalColumn: "GcId");
                    table.ForeignKey(
                        name: "FK_GGGuess_GGRoundScore_roundScoreId",
                        column: x => x.roundScoreId,
                        principalTable: "GGRoundScore",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GGGuess_GGTotalDistance_distanceId",
                        column: x => x.distanceId,
                        principalTable: "GGTotalDistance",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NextId = table.Column<int>(type: "INTEGER", nullable: true),
                    Channel = table.Column<string>(type: "TEXT", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GeoGuessrId = table.Column<string>(type: "TEXT", nullable: true),
                    IsUsStreak = table.Column<bool>(type: "INTEGER", nullable: false),
                    BoundsId = table.Column<int>(type: "INTEGER", nullable: true),
                    Flags = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Mode = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentRound = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionInChainFromStart = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.UniqueConstraint("AK_Game_Id_Channel", x => new { x.Id, x.Channel });
                    table.ForeignKey(
                        name: "FK_Game_Bounds_BoundsId",
                        column: x => x.BoundsId,
                        principalTable: "Bounds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Game_Game_NextId",
                        column: x => x.NextId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_GeoGuessrGame_SourceId",
                        column: x => x.SourceId,
                        principalTable: "GeoGuessrGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GGRound",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    lat = table.Column<double>(type: "REAL", nullable: false),
                    lng = table.Column<double>(type: "REAL", nullable: false),
                    panoId = table.Column<string>(type: "TEXT", nullable: true),
                    heading = table.Column<double>(type: "REAL", nullable: false),
                    pitch = table.Column<double>(type: "REAL", nullable: false),
                    zoom = table.Column<double>(type: "REAL", nullable: false),
                    streakLocationCode = table.Column<string>(type: "TEXT", nullable: true),
                    GeoGuessrGameId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GGRound", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GGRound_GeoGuessrGame_GeoGuessrGameId",
                        column: x => x.GeoGuessrGameId,
                        principalTable: "GeoGuessrGame",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Channel = table.Column<string>(type: "TEXT", nullable: false),
                    PlatformId = table.Column<string>(type: "TEXT", nullable: true),
                    SourcePlatform = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: true),
                    PlayerFlag = table.Column<string>(type: "TEXT", nullable: true),
                    PlayerFlagName = table.Column<string>(type: "TEXT", nullable: true),
                    CountryStreak = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    BestStreak = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CorrectCountries = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    NumberOfCountries = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Perfects = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    BestGame = table.Column<double>(type: "REAL", nullable: false, defaultValue: 0.0),
                    BestRound = table.Column<double>(type: "REAL", nullable: false, defaultValue: 0.0),
                    SumOfGuesses = table.Column<double>(type: "REAL", nullable: false),
                    TotalDistance = table.Column<double>(type: "REAL", nullable: false),
                    LastGuess = table.Column<string>(type: "TEXT", nullable: true),
                    NoOfGuesses = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    NoOf5kGuesses = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    LastGameId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoundNumberOfLastGuess = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    IsBanned = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    StreakBefore = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstGuessMade = table.Column<bool>(type: "INTEGER", nullable: false),
                    IdOfLastGame = table.Column<int>(type: "INTEGER", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.UniqueConstraint("AK_Players_Id_Channel", x => new { x.Id, x.Channel });
                    table.ForeignKey(
                        name: "FK_Players_Game_LastGameId",
                        column: x => x.LastGameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorrectLocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoundNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Flags = table.Column<int>(type: "INTEGER", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: true),
                    ExactCountryId = table.Column<int>(type: "INTEGER", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Round_Coordinates_CorrectLocationId",
                        column: x => x.CorrectLocationId,
                        principalTable: "Coordinates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Round_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Round_Country_ExactCountryId",
                        column: x => x.ExactCountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Round_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuessCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: true),
                    Distance = table.Column<double>(type: "REAL", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    Streak = table.Column<int>(type: "INTEGER", nullable: false),
                    Time = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameResults_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameResults_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Guess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<double>(type: "REAL", nullable: false),
                    GuessLocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Distance = table.Column<double>(type: "REAL", nullable: false),
                    GuessedBefore = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsStreamerGuess = table.Column<bool>(type: "INTEGER", nullable: false),
                    WasRandom = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTemporary = table.Column<bool>(type: "INTEGER", nullable: false),
                    RoundId = table.Column<int>(type: "INTEGER", nullable: true),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    Streak = table.Column<int>(type: "INTEGER", nullable: false),
                    Pano = table.Column<string>(type: "TEXT", nullable: true),
                    Heading = table.Column<double>(type: "REAL", nullable: false),
                    Pitch = table.Column<double>(type: "REAL", nullable: false),
                    FOV = table.Column<double>(type: "REAL", nullable: false),
                    Zoom = table.Column<double>(type: "REAL", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: true),
                    CountryExactId = table.Column<int>(type: "INTEGER", nullable: true),
                    GuessCounter = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    Bot = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guess_Coordinates_GuessLocationId",
                        column: x => x.GuessLocationId,
                        principalTable: "Coordinates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guess_Country_CountryExactId",
                        column: x => x.CountryExactId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guess_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guess_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Guess_Round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Round",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoundResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoundId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    Distance = table.Column<double>(type: "REAL", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    Time = table.Column<double>(type: "REAL", nullable: false),
                    Streak = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoundResult_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoundResult_Round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Round",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bounds_MaxId",
                table: "Bounds",
                column: "MaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Bounds_MinId",
                table: "Bounds",
                column: "MinId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_BoundsId",
                table: "Game",
                column: "BoundsId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_NextId",
                table: "Game",
                column: "NextId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_SourceId",
                table: "Game",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_GameResults_GameId",
                table: "GameResults",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameResults_PlayerId",
                table: "GameResults",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoGuessrGame_boundsId",
                table: "GeoGuessrGame",
                column: "boundsId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoGuessrGame_playerGcId",
                table: "GeoGuessrGame",
                column: "playerGcId");

            migrationBuilder.CreateIndex(
                name: "IX_GGBounds_maxId",
                table: "GGBounds",
                column: "maxId");

            migrationBuilder.CreateIndex(
                name: "IX_GGBounds_minId",
                table: "GGBounds",
                column: "minId");

            migrationBuilder.CreateIndex(
                name: "IX_GGGuess_distanceId",
                table: "GGGuess",
                column: "distanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GGGuess_GGPlayerGcId",
                table: "GGGuess",
                column: "GGPlayerGcId");

            migrationBuilder.CreateIndex(
                name: "IX_GGGuess_roundScoreId",
                table: "GGGuess",
                column: "roundScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_GGPlayer_pinId",
                table: "GGPlayer",
                column: "pinId");

            migrationBuilder.CreateIndex(
                name: "IX_GGPlayer_totalDistanceId",
                table: "GGPlayer",
                column: "totalDistanceId");

            migrationBuilder.CreateIndex(
                name: "IX_GGPlayer_totalScoreId",
                table: "GGPlayer",
                column: "totalScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_GGRound_GeoGuessrGameId",
                table: "GGRound",
                column: "GeoGuessrGameId");

            migrationBuilder.CreateIndex(
                name: "IX_GGTotalDistance_metersId",
                table: "GGTotalDistance",
                column: "metersId");

            migrationBuilder.CreateIndex(
                name: "IX_GGTotalDistance_milesId",
                table: "GGTotalDistance",
                column: "milesId");

            migrationBuilder.CreateIndex(
                name: "IX_Guess_CountryExactId",
                table: "Guess",
                column: "CountryExactId");

            migrationBuilder.CreateIndex(
                name: "IX_Guess_CountryId",
                table: "Guess",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Guess_GuessLocationId",
                table: "Guess",
                column: "GuessLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Guess_PlayerId",
                table: "Guess",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Guess_RoundId",
                table: "Guess",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_LastGameId",
                table: "Players",
                column: "LastGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_CorrectLocationId",
                table: "Round",
                column: "CorrectLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_CountryId",
                table: "Round",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_ExactCountryId",
                table: "Round",
                column: "ExactCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_GameId",
                table: "Round",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundResult_PlayerId",
                table: "RoundResult",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundResult_RoundId",
                table: "RoundResult",
                column: "RoundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "GameResults");

            migrationBuilder.DropTable(
                name: "GGGuess");

            migrationBuilder.DropTable(
                name: "GGRound");

            migrationBuilder.DropTable(
                name: "Guess");

            migrationBuilder.DropTable(
                name: "RoundResult");

            migrationBuilder.DropTable(
                name: "GGRoundScore");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Bounds");

            migrationBuilder.DropTable(
                name: "GeoGuessrGame");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "GGBounds");

            migrationBuilder.DropTable(
                name: "GGPlayer");

            migrationBuilder.DropTable(
                name: "GGMax");

            migrationBuilder.DropTable(
                name: "GGMin");

            migrationBuilder.DropTable(
                name: "GGPin");

            migrationBuilder.DropTable(
                name: "GGTotalDistance");

            migrationBuilder.DropTable(
                name: "GGTotalScore");

            migrationBuilder.DropTable(
                name: "GGMeters");

            migrationBuilder.DropTable(
                name: "GGMiles");
        }
    }
}
