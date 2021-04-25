using Microsoft.EntityFrameworkCore.Migrations;

namespace data.Migrations
{
    public partial class StatusMoneyPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Coefficient",
                table: "Ticket",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PairTip",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PairTip");

            migrationBuilder.AlterColumn<int>(
                name: "Coefficient",
                table: "Ticket",
                type: "integer",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
