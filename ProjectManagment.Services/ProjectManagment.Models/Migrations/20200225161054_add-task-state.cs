using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagment.Models.Migrations
{
    public partial class addtaskstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "Task",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "Task");
        }
    }
}
