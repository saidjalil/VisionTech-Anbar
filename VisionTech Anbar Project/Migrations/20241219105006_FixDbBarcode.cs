using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisionTech_Anbar_Project.Migrations
{
    public partial class FixDbBarcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barcodes");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "PackageProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "PackageProducts");

            migrationBuilder.CreateTable(
                name: "Barcodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barcodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Barcodes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Barcodes_BarCode",
                table: "Barcodes",
                column: "BarCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Barcodes_ProductId",
                table: "Barcodes",
                column: "ProductId");
        }
    }
}
