using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RockSteadyGo.Core.Api.Migrations
{
    public partial class AddMoves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationX = table.Column<int>(type: "int", nullable: false),
                    LocationY = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moves_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Moves_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Moves",
                columns: new[] { "Id", "CreatedDate", "LocationX", "LocationY", "MatchId", "PlayerId", "Type" },
                values: new object[,]
                {
                    { new Guid("1da5dc08-1294-459e-8c63-e3d846acd170"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 10, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 2, new Guid("445b3d2c-2d69-4ae0-9b0a-269c213c35d1"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef1"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 5, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 2, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef2"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 10, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 0, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 0 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef3"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 15, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 2, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef4"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 20, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 2, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 0 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef5"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 25, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 1, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef6"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 30, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 1, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 0 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef7"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 35, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 0, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef8"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 1, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 0 },
                    { new Guid("a7c37018-3459-49cb-852f-20a74eed4ef9"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 5, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 0, new Guid("e4fc700e-a737-40e9-82d8-ffb85795ea61"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24281"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 5, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 2, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24282"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 10, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 0, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("edaf90dc-1e0f-4d6c-9871-93148f081b1c"), 0 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24283"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 15, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 2, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24284"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 20, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 2, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("edaf90dc-1e0f-4d6c-9871-93148f081b1c"), 0 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24285"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 25, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, 1, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24286"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 30, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 1, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("edaf90dc-1e0f-4d6c-9871-93148f081b1c"), 0 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24287"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 35, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, 0, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("ca66ac15-7b4f-4a4a-a51d-337983545900"), 1 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24288"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 40, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 1, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("edaf90dc-1e0f-4d6c-9871-93148f081b1c"), 0 },
                    { new Guid("c0c7da2e-ac6b-46f5-9688-678842d24289"), new DateTimeOffset(new DateTime(2020, 2, 2, 0, 0, 45, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0, 0, new Guid("f1cc8633-6999-40f2-90b5-4b37c824318f"), new Guid("edaf90dc-1e0f-4d6c-9871-93148f081b1c"), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moves_MatchId",
                table: "Moves",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PlayerId",
                table: "Moves",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moves");
        }
    }
}
