using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagment.Models.Migrations
{
    public partial class addactiveuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "User");
        }
    }
}
