using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCProject.Data.Migrations
{
    public partial class ResetGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Game",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Studio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Support = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Size = table.Column<float>(type: "real", nullable: false),
                Score = table.Column<int>(type: "int", nullable: false),
                Engine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                Budget = table.Column<float>(type: "real", nullable: false),
                Statut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
