using Microsoft.EntityFrameworkCore.Migrations;

namespace data.Migrations
{
    public partial class MoneyPayed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PrizeAmount",
                table: "Ticket",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<decimal>(
                name: "MoneyPaid",
                table: "Ticket",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoneyPaid",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "PrizeAmount",
                table: "Ticket",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
