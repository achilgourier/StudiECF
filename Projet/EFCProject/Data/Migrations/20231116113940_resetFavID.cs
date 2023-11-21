using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCProject.Data.Migrations
{
    public partial class resetFavID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Favorit",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false), // Utilisez la longueur appropriée
                GameId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Favorit", x => x.Id);
            });



            // Ajouter une contrainte unique sur UserId et GameId
            migrationBuilder.CreateIndex(
                name: "IX_Favorit_UserId_GameId",
                table: "Favorit",
                columns: new[] { "UserId", "GameId" },
                unique: true);

            // Ajouter une clé étrangère si UserId fait référence à une autre table
            migrationBuilder.AddForeignKey(
                name: "FK_Favorit_Users_UserId",
                table: "Favorit",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorit");
        }
    }
}
