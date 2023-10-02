using Microsoft.EntityFrameworkCore.Migrations;

namespace Down_Notifier_Web.Data.Migrations
{
    public partial class addEmailAdress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "HealthChecks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "HealthChecks");
        }
    }
}
