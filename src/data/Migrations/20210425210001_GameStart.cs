using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace data.Migrations
{
    public partial class GameStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportTip_SportTeam_SportTeamId",
                table: "SportTip");

            migrationBuilder.DropIndex(
                name: "IX_SportTip_SportTeamId",
                table: "SportTip");

            migrationBuilder.DropColumn(
                name: "SportTeamId",
                table: "SportTip");

            migrationBuilder.AddColumn<DateTime>(
                name: "GameStart",
                table: "Pair",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameStart",
                table: "Pair");

            migrationBuilder.AddColumn<int>(
                name: "SportTeamId",
                table: "SportTip",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SportTip_SportTeamId",
                table: "SportTip",
                column: "SportTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_SportTip_SportTeam_SportTeamId",
                table: "SportTip",
                column: "SportTeamId",
                principalTable: "SportTeam",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
