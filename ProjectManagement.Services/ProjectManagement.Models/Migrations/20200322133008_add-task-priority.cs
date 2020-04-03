using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Models.Migrations
{
    public partial class addtaskpriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "Task",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "priority",
                table: "Task");
        }
    }
}
