// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RockSteadyGo.Core.Api.Migrations
{
    public partial class AddPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "Name", "Username" },
                values: new object[] { new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), new DateTimeOffset(new DateTime(2020, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Hassan Habib", "hhabib" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "Name", "Username" },
                values: new object[] { new Guid("edaf90dc-1e0f-4d6c-9871-93148f081b1c"), new DateTimeOffset(new DateTime(2020, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Etienne Cowley", "ecowley" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
