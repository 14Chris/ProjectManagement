using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagment.Models.Migrations
{
    public partial class adddescproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "Task",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Project");
        }
    }
}
