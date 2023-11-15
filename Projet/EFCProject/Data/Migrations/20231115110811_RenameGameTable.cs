using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCProject.Data.Migrations
{
    public partial class RenameGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
