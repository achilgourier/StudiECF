using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCProject.Data.Migrations
{
    public partial class UpdateDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Game",
                type: "DATETIME2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Game",
                type: "DATETIME2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
