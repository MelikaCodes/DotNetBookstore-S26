using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetBookstore.Data.Migrations
{
    /// <inheritdoc />
    public partial class StorefrontCheckout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceCents",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<string>(type: "TEXT", nullable: false),
                    CheckoutId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlacedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLine",
                columns: table => new
                {
                    OrderLineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPriceCents = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLine", x => x.OrderLineId);
                    table.ForeignKey(
                        name: "FK_OrderLine_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                column: "PriceCents",
                value: 1299);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                column: "PriceCents",
                value: 1099);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                column: "PriceCents",
                value: 999);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4,
                column: "PriceCents",
                value: 899);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 5,
                column: "PriceCents",
                value: 1599);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 6,
                column: "PriceCents",
                value: 1499);

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_OrderId",
                table: "OrderLine",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId_CheckoutId",
                table: "Orders",
                columns: new[] { "CustomerId", "CheckoutId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLine");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "PriceCents",
                table: "Books");
        }
    }
}
