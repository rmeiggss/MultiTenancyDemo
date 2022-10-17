﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenancyDemo.Data.Migrations
{
    public partial class multitenancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Pais",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 1, "Republica Dominicana" });

            migrationBuilder.InsertData(
                table: "Pais",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 2, "Mexico" });

            migrationBuilder.InsertData(
                table: "Pais",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 3, "Colombia" });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_TenantId",
                table: "Productos",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pais");

            migrationBuilder.DropTable(
                name: "Productos");
        }
    }
}
