using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SallaJo.Migrations
{
    /// <inheritdoc />
    public partial class contactTypeMigratio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactTypeId",
                table: "Stores",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ContactTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "whatsapp" },
                    { 2, "instagram" },
                    { 3, "facebook" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ContactTypeId",
                table: "Stores",
                column: "ContactTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_ContactTypes_ContactTypeId",
                table: "Stores",
                column: "ContactTypeId",
                principalTable: "ContactTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_ContactTypes_ContactTypeId",
                table: "Stores");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ContactTypeId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ContactTypeId",
                table: "Stores");
        }
    }
}
