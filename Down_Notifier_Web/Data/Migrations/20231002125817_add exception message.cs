using Microsoft.EntityFrameworkCore.Migrations;

namespace Down_Notifier_Web.Data.Migrations
{
    public partial class addexceptionmessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "HealthCheckMailLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "HealthCheckLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "HealthCheckMailLogs");

            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "HealthCheckLogs");
        }
    }
}
