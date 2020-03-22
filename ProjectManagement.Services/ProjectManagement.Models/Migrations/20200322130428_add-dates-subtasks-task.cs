using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Models.Migrations
{
    public partial class adddatessubtaskstask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "creation_date",
                table: "Task",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "due_date",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_main_task",
                table: "Task",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                table: "Task",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_id_main_task",
                table: "Task",
                column: "id_main_task");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Task_id_main_task",
                table: "Task",
                column: "id_main_task",
                principalTable: "Task",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Task_id_main_task",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_id_main_task",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "creation_date",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "due_date",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "id_main_task",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "start_date",
                table: "Task");
        }
    }
}
