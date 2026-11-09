using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fbr_full.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HSCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UOM",
                table: "Products",
                newName: "Unit");

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxStockLevel",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinStockLevel",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RetailPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StockQuantity",
                table: "Products",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6204));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6207));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6208));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6209));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6065));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6069));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6070));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6071));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6072));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 44, 56, 255, DateTimeKind.Utc).AddTicks(6073));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaxStockLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinStockLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RetailPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Products",
                newName: "UOM");

            migrationBuilder.AddColumn<string>(
                name: "HSCode",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(598));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(601));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(602));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(603));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(426));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(432));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(434));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(435));

            migrationBuilder.UpdateData(
                table: "TaxRates",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 3, 21, 12, 40, 212, DateTimeKind.Utc).AddTicks(436));
        }
    }
}
