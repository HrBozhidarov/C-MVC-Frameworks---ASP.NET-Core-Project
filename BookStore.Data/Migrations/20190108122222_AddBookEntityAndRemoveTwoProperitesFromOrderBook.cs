using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Data.Migrations
{
    public partial class AddBookEntityAndRemoveTwoProperitesFromOrderBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookName",
                table: "OrdersBooks");

            migrationBuilder.DropColumn(
                name: "BookPrice",
                table: "OrdersBooks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookName",
                table: "OrdersBooks",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BookPrice",
                table: "OrdersBooks",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
