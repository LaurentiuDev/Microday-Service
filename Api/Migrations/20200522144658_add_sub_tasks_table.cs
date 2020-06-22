using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class add_sub_tasks_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Task",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SubTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 512, nullable: false),
                    Priority = table.Column<int>(nullable: true),
                    Completed = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTask_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_UserId",
                table: "Task",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTask_TaskId",
                table: "SubTask",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_UserId",
                table: "Task",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_UserId",
                table: "Task");

            migrationBuilder.DropTable(
                name: "SubTask");

            migrationBuilder.DropIndex(
                name: "IX_Task_UserId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Task");
        }
    }
}
