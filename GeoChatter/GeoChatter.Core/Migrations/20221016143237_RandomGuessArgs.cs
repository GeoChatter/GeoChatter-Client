using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoChatter.Core.Migrations
{
    public partial class RandomGuessArgs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RandomGuessArgs",
                table: "Guess",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandomGuessArgs",
                table: "Guess");
        }
    }
}
