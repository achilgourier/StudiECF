﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCProject.Data.Migrations
{
    public partial class nouvelleMigrationLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "ModificationLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "ModificationLog",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

        }
    }
}
