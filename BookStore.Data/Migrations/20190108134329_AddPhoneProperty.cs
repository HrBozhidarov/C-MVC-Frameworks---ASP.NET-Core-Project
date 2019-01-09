using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Data.Migrations
{
    public partial class AddPhoneProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Orders");
        }
    }
}
