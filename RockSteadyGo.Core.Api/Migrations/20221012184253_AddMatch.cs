using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RockSteadyGo.Core.Api.Migrations
{
    public partial class AddMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Match",
                columns: new[] { "Id", "CreatedDate" },
                values: new object[] { new Guid("445b3d2c-2d69-4ae0-9b0a-269c213c35d1"), new DateTimeOffset(new DateTime(2020, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Match",
                columns: new[] { "Id", "CreatedDate" },
                values: new object[] { new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new DateTimeOffset(new DateTime(2020, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Match",
                columns: new[] { "Id", "CreatedDate" },
                values: new object[] { new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new DateTimeOffset(new DateTime(2020, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");
        }
    }
}
