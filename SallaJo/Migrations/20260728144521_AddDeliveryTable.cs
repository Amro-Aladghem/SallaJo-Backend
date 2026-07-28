using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SallaJo.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHasDelivery",
                table: "Stores",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StoreDeliveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GovernorateId = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDelivered = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreDeliveries_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreDeliveries_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_StoreDeliveries_GovernorateId",
                table: "StoreDeliveries",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreDeliveries_StoreId",
                table: "StoreDeliveries",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreDeliveries");

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IsHasDelivery",
                table: "Stores");
        }
    }
}
