using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisionTech_Anbar_Project.Migrations
{
    public partial class FixDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageProducts",
                table: "PackageProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageProducts",
                table: "PackageProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageProducts_PackageId",
                table: "PackageProducts",
                column: "PackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PackageProducts",
                table: "PackageProducts");

            migrationBuilder.DropIndex(
                name: "IX_PackageProducts_PackageId",
                table: "PackageProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackageProducts",
                table: "PackageProducts",
                columns: new[] { "PackageId", "ProductId" });
        }
    }
}
